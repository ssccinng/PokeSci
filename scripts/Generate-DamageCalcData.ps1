param(
    [string]$OutputPath = (Join-Path $PSScriptRoot '..\Poke.DamageCalc\Data\GeneratedDamageData.g.cs'),
    [string]$PackageVersion = '0.11.0',
    [string]$DamageCalcRepository = 'https://github.com/smogon/damage-calc.git',
    [string]$DamageCalcSetsPath = 'src/js/data/sets/gen9.js'
)

$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$resolvedOutputPath = if ([System.IO.Path]::IsPathRooted($OutputPath)) {
    $OutputPath
} else {
    Join-Path $repoRoot $OutputPath
}
$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) ('poke-damage-calc-codegen-' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $tempRoot | Out-Null

try {
    Push-Location $tempRoot
    npm init -y | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "npm init failed with exit code $LASTEXITCODE."
    }

    npm install "@smogon/calc@$PackageVersion" --silent | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "npm install @smogon/calc@$PackageVersion failed with exit code $LASTEXITCODE."
    }

    git clone --depth 1 $DamageCalcRepository damage-calc-src --quiet
    if ($LASTEXITCODE -ne 0) {
        throw "git clone $DamageCalcRepository failed with exit code $LASTEXITCODE."
    }

    $setsPath = Join-Path $tempRoot (Join-Path 'damage-calc-src' $DamageCalcSetsPath)

    $nodeScript = @'
const fs = require('fs');
const path = require('path');
const calc = require('@smogon/calc');

const output = process.argv[2];
const setsPath = process.argv[3];
const pkg = require('@smogon/calc/package.json');
const speciesRaw = calc.SPECIES[9];
const movesRaw = calc.MOVES[9];
const itemsRaw = calc.ITEMS[9];
const abilitiesRaw = calc.ABILITIES[9];
const naturesRaw = calc.NATURES;
const typeChartRaw = calc.TYPE_CHART[9];
const setDexRaw = loadSetDex(setsPath);

function toId(value) {
  return String(value || '').toLowerCase().replace(/[^a-z0-9]/g, '');
}

function csString(value) {
  return '@"' + String(value ?? '').replace(/"/g, '""') + '"';
}

function csStringArray(values) {
  if (!values || values.length === 0) return '[]';
  return '[' + values.map(csString).join(', ') + ']';
}

function csArrayInitializer(values, indent) {
  if (!values || values.length === 0) return '';
  return values.map(value => indent + csString(value)).join(',\n');
}

const DATA_CHUNK_SIZE = 200;

function chunk(values, size = DATA_CHUNK_SIZE) {
  const chunks = [];
  for (let index = 0; index < values.length; index += size) {
    chunks.push(values.slice(index, index + size));
  }

  return chunks;
}

function csDictionaryAssignment(entry) {
  return String(entry).replace(/^\s*\[([^\]]+)\]\s*=\s*/, '        data[$1] = ') + ';';
}

function csValueExpression(entry) {
  const text = String(entry).trim();
  return text.endsWith(',') ? text.slice(0, -1) : text;
}

function csDictionaryLazy(name, valueType, entries) {
  const chunks = chunk(entries);
  const addCalls = chunks.map((_, index) => `        Add${name}${index}(data);`).join('\n');
  const methods = chunks.map((entriesChunk, index) => `    private static void Add${name}${index}(Dictionary<string, ${valueType}> data)
    {
${entriesChunk.map(csDictionaryAssignment).join('\n')}
    }`).join('\n\n');

  return `    private static Dictionary<string, ${valueType}> ${name} => ${name}Store.Value;

    private static readonly Lazy<Dictionary<string, ${valueType}>> ${name}Store = new(Create${name});

    private static Dictionary<string, ${valueType}> Create${name}()
    {
        var data = new Dictionary<string, ${valueType}>(StringComparer.OrdinalIgnoreCase);
${addCalls ? addCalls + '\n' : ''}        return data;
    }

${methods}`;
}

function csArrayLazy(name, valueType, entries) {
  const chunks = chunk(entries);
  const addCalls = chunks.map((_, index) => `        Add${name}${index}(values);`).join('\n');
  const methods = chunks.map((entriesChunk, index) => `    private static void Add${name}${index}(List<${valueType}> values)
    {
${entriesChunk.map(entry => `        values.Add(${csValueExpression(entry)});`).join('\n')}
    }`).join('\n\n');

  return `    private static ${valueType}[] ${name} => ${name}Store.Value;

    private static readonly Lazy<${valueType}[]> ${name}Store = new(Create${name});

    private static ${valueType}[] Create${name}()
    {
        var values = new List<${valueType}>(${entries.length});
${addCalls ? addCalls + '\n' : ''}        return values.ToArray();
    }

${methods}`;
}

function moveCategory(value, bp) {
  const category = value || (bp === 0 ? 'Status' : 'Physical');
  if (category === 'Special') return 'MoveCategory.Special';
  if (category === 'Status') return 'MoveCategory.Status';
  return 'MoveCategory.Physical';
}

function moveTarget(value) {
  switch (value) {
    case 'allAdjacent': return 'MoveTarget.AllAdjacent';
    case 'allAdjacentFoes': return 'MoveTarget.AllAdjacentFoes';
    case 'self': return 'MoveTarget.Self';
    case 'normal': return 'MoveTarget.Normal';
    default: return 'MoveTarget.Any';
  }
}

function moveFlags(data) {
  const flags = [];
  if (data.makesContact || data.flags?.contact) flags.push('Contact = true');
  if (data.isPunch || data.flags?.punch) flags.push('Punch = true');
  if (data.isSound || data.flags?.sound) flags.push('Sound = true');
  if (data.isBite || data.flags?.bite) flags.push('Bite = true');
  if (data.isBullet || data.flags?.bullet) flags.push('Bullet = true');
  if (data.isPulse || data.flags?.pulse) flags.push('Pulse = true');
  if (data.isSlicing || data.flags?.slicing) flags.push('Slicing = true');
  if (data.isWind || data.flags?.wind) flags.push('Wind = true');
  return flags.length === 0 ? 'null' : 'new MoveFlags { ' + flags.join(', ') + ' }';
}

function multihit(data) {
  if (!data.multihit) return 'null';
  if (Array.isArray(data.multihit)) return `(${data.multihit[0]}, ${data.multihit[1]})`;
  return `(${data.multihit}, ${data.multihit})`;
}

function tuple2(value) {
  return value ? `(${value[0]}, ${value[1]})` : 'null';
}

function loadSetDex(file) {
  const vm = require('vm');
  const source = fs.readFileSync(file, 'utf8');
  const context = {};
  vm.createContext(context);
  vm.runInContext(source + '; this.SETDEX_SV = SETDEX_SV;', context, {filename: file});
  return context.SETDEX_SV || {};
}

function statTable(data, defaultValue) {
  const stats = data || {};
  return `new StatsTable(${stats.hp ?? defaultValue}, ${stats.at ?? defaultValue}, ${stats.df ?? defaultValue}, ${stats.sa ?? defaultValue}, ${stats.sd ?? defaultValue}, ${stats.sp ?? defaultValue})`;
}

function nullableString(value) {
  return value ? csString(value) : 'null';
}

function nullableInt(value) {
  return value === undefined || value === null ? 'null' : String(value);
}

function inferFormat(setName) {
  const match = String(setName).match(/^(National Dex Doubles|National Dex Ubers|National Dex UU|National Dex RU|National Dex Monotype|National Dex|Balanced Hackmons|Almost Any Ability|Anything Goes|Monotype|Ubers UU|BSS Reg J|OU|UU|RU|NU|PU|ZU|NFE|LC)\b/);
  return match ? match[1] : null;
}

const speciesEntries = Object.entries(speciesRaw)
  .map(([name, data]) => {
    const id = toId(name);
    const bs = data.bs || {};
    const hp = bs.hp ?? 1;
    const atk = bs.at ?? 1;
    const def = bs.df ?? 1;
    const spa = bs.sa ?? bs.sl ?? 1;
    const spd = bs.sd ?? bs.sl ?? 1;
    const spe = bs.sp ?? 1;
    const types = data.types || ['Normal'];
    const abilities = data.abilities ? Object.values(data.abilities).filter(Boolean) : [];
    return `        [${csString(id)}] = new SpeciesData(${csString(name)}, new StatsTable(${hp}, ${atk}, ${def}, ${spa}, ${spd}, ${spe}), ${csStringArray(types)}, ${Number(data.weightkg || 0).toString()}, ${csStringArray(abilities)}, ${data.nfe ? 'true' : 'false'})`;
  })
  .sort();

const moveEntries = Object.entries(movesRaw)
  .map(([name, data]) => {
    const id = toId(name);
    const bp = data.bp ?? 0;
    return `        [${csString(id)}] = new MoveData(${csString(data.name || name)}, ${bp}, ${csString(data.type || 'Normal')}, ${moveCategory(data.category, bp)}, ${moveFlags(data)}, ${moveTarget(data.target)}, ${data.priority || 0}, ${multihit(data)}, ${tuple2(data.recoil)}, ${tuple2(data.drain)})`;
  })
  .sort();

const speciesOptions = Object.entries(speciesRaw)
  .map(([name, data]) => {
    const id = toId(name);
    const types = data.types || ['Normal'];
    return `        new SpeciesOption(${csString(id)}, ${csString(name)}, null, [], ${csStringArray(types)})`;
  })
  .sort();

const moveOptions = Object.entries(movesRaw)
  .map(([name, data]) => {
    const id = toId(name);
    const bp = data.bp ?? 0;
    return `        new MoveOption(${csString(id)}, ${csString(data.name || name)}, [], ${csString(data.type || 'Normal')}, ${moveCategory(data.category, bp)}, ${bp})`;
  })
  .sort();

const setEntries = [];
const setOptions = [];
for (const [speciesName, sets] of Object.entries(setDexRaw)) {
  const speciesId = toId(speciesName);
  for (const [setName, set] of Object.entries(sets)) {
    const key = speciesId + '|' + toId(setName);
    const format = inferFormat(setName);
    setEntries.push(`        [${csString(key)}] = new PokemonSetData(${csString(speciesId)}, ${csString(speciesName)}, ${csString(setName)}, ${nullableString(set.ability)}, ${nullableString(set.item)}, ${nullableString(set.nature)}, ${nullableString(set.teraType)}, ${nullableInt(set.level)}, ${statTable(set.evs, 0)}, ${statTable(set.ivs, 31)}, ${csStringArray(set.moves || [])}, ${nullableString(format)})`);
    setOptions.push(`        new PokemonSetOption(${csString(speciesId)}, ${csString(speciesName)}, ${csString(setName)}, ${nullableString(format)})`);
  }
}
setEntries.sort();
setOptions.sort();

const generatedItems = Object.values(itemsRaw)
  .filter(value => typeof value === 'string' && value.length > 0)
  .sort();

const generatedAbilities = Object.values(abilitiesRaw)
  .filter(value => typeof value === 'string' && value.length > 0)
  .sort();

const generatedNatures = Object.keys(naturesRaw)
  .filter(value => value.length > 0)
  .sort();

const generatedTypes = Object.keys(typeChartRaw)
  .filter(value => value !== '???')
  .sort();

const typeChartEntries = Object.entries(typeChartRaw)
  .map(([attackingType, row]) => {
    const rowEntries = Object.entries(row)
      .map(([defendingType, multiplier]) => `            [${csString(defendingType)}] = ${Number(multiplier).toString()}`)
      .sort();

    return `        [${csString(attackingType)}] = new(StringComparer.OrdinalIgnoreCase)\n        {\n${rowEntries.join(',\n')}\n        }`;
  })
  .sort();

const content = `// <auto-generated />
// Generated by scripts/Generate-DamageCalcData.ps1 from @smogon/calc ${pkg.version}.
namespace Poke.DamageCalc.Data;

public static partial class DamageData
{
    public const string GeneratedPackage = "@smogon/calc";
    public const string GeneratedPackageVersion = "${pkg.version}";
    public const int GeneratedGeneration = 9;

${csDictionaryLazy('GeneratedSpeciesById', 'SpeciesData', speciesEntries)}

${csDictionaryLazy('GeneratedMovesById', 'MoveData', moveEntries)}

${csArrayLazy('GeneratedSpeciesOptions', 'SpeciesOption', speciesOptions)}

${csArrayLazy('GeneratedMoveOptions', 'MoveOption', moveOptions)}

${csDictionaryLazy('GeneratedPokemonSetsByKey', 'PokemonSetData', setEntries)}

${csArrayLazy('GeneratedPokemonSetOptions', 'PokemonSetOption', setOptions)}

${csArrayLazy('GeneratedItems', 'string', generatedItems.map(csString))}

${csArrayLazy('GeneratedAbilities', 'string', generatedAbilities.map(csString))}

${csArrayLazy('GeneratedNatures', 'string', generatedNatures.map(csString))}

${csArrayLazy('GeneratedTypes', 'string', generatedTypes.map(csString))}

    private static partial bool TryGetGeneratedSpecies(string id, out SpeciesData species) =>
        GeneratedSpeciesById.TryGetValue(id, out species!);

    private static partial bool TryGetGeneratedMove(string id, out MoveData move) =>
        GeneratedMovesById.TryGetValue(id, out move!);

    private static partial bool TryGetGeneratedPokemonSet(string speciesId, string setName, out PokemonSetData set) =>
        GeneratedPokemonSetsByKey.TryGetValue(speciesId + "|" + Ids.ToId(setName), out set!);

    private static partial bool HasGeneratedSpecies(string id) => GeneratedSpeciesById.ContainsKey(id);

    private static partial bool HasGeneratedMove(string id) => GeneratedMovesById.ContainsKey(id);

    private static partial bool TryResolveGeneratedSpeciesAlias(string nameOrAlias, out string id)
    {
        id = Ids.ToId(nameOrAlias);
        return GeneratedSpeciesById.ContainsKey(id);
    }

    private static partial bool TryResolveGeneratedMoveAlias(string nameOrAlias, out string id)
    {
        id = Ids.ToId(nameOrAlias);
        return GeneratedMovesById.ContainsKey(id);
    }

    private static partial bool TryResolveGeneratedSpeciesDex(int nationalDexId, out string id)
    {
        id = string.Empty;
        return false;
    }

    private static partial IReadOnlyList<SpeciesOption> GetGeneratedSpeciesOptions() => GeneratedSpeciesOptions;

    private static partial IReadOnlyList<MoveOption> GetGeneratedMoveOptions() => GeneratedMoveOptions;

    private static partial IReadOnlyList<PokemonSetOption> GetGeneratedPokemonSetOptions() => GeneratedPokemonSetOptions;

    private static partial IReadOnlyList<string> GetGeneratedItems() => GeneratedItems;

    private static partial IReadOnlyList<string> GetGeneratedAbilities() => GeneratedAbilities;

    private static partial IReadOnlyList<string> GetGeneratedNatures() => GeneratedNatures;

    private static partial IReadOnlyList<string> GetGeneratedTypes() => GeneratedTypes;
}

public static partial class TypeChart
{
    private static readonly Dictionary<string, Dictionary<string, double>> GeneratedTypeChart = new(StringComparer.OrdinalIgnoreCase)
    {
${typeChartEntries.join(',\n')}
    };

    private static partial bool TryGetGeneratedEffectiveness(string attackingType, string defendingType, out double value)
    {
        if (GeneratedTypeChart.TryGetValue(attackingType, out var row) &&
            row.TryGetValue(defendingType, out value))
        {
            return true;
        }

        value = 1;
        return false;
    }
}
`;

fs.mkdirSync(path.dirname(output), {recursive: true});
fs.writeFileSync(output, content, 'utf8');
'@

    Set-Content -Path .\generate.cjs -Value $nodeScript -Encoding UTF8
    node .\generate.cjs $resolvedOutputPath $setsPath
    if ($LASTEXITCODE -ne 0) {
        throw "node data generation failed with exit code $LASTEXITCODE."
    }
}
finally {
    Pop-Location
    if (Test-Path $tempRoot) {
        Remove-Item -LiteralPath $tempRoot -Recurse -Force
    }
}

Write-Host "Generated $resolvedOutputPath"
