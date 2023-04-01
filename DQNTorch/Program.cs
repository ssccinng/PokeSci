using DQNTorch;

var agent = new DQNAgent(new PokeDanEnvTest());

agent.train(1000);