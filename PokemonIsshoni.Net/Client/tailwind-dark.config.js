const colors = require('tailwindcss/colors')

module.exports = {
    mode: 'jit',
    important: false,

    prefix: 'tw-',
    purge: [
        './wwwroot/index.html',
        './**/*.razor',
        './Areas/Identity/Pages/Account/*.cshtml'
    ],
    darkMode: 'media',
    theme: {
        extend: {
            colors: {
                cyan: colors.cyan
            }
        },
    },
    variants: {
        extend: {
            backgroundColor: ['active'],
            visibility: ['hover', 'focus'],
        }
    },
    corePlugins: {
        // ...
        // displayhidden: false,
    },

    plugins: [
        // require('tailwindcss-textshadow')
    ]
}