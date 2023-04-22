// See https://aka.ms/new-console-template for more information
using DQNTorch;

Console.WriteLine("Hello, World!");
PokeDanLadder pokeDanLadder = new(2);
pokeDanLadder.train(10000);
pokeDanLadder.SaveAll();

//await zQDQNAgent.train(1);
Console.ReadLine();
