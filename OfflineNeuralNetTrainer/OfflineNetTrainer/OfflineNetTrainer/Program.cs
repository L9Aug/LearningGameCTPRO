// core neural net functionality based on work from : http://neuralnetworksanddeeplearning.com/chap1.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NeuralNet;

namespace OfflineNetTrainer
{
    class Program
    {
        public float[] Inputs;
        public float[] Outputs;
        public float[] ExpectedOutputs;

        public int NumTrainingCycles;
        public int TrainingBatchSize;
        public float LearningRate = 0.1f;
        public int NumLayers = 4;
        public int[] LayerNeuronCounts = { 5, 200, 200, 7 };
        NeuronLayer[] NetLayers;

        static void Main(string[] args)
        {
            Program myProg = new Program();
            myProg.SetupNet();
            Console.WriteLine("Enter the number of Training cycles to perform:");
            myProg.NumTrainingCycles = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Batch Size:");
            myProg.TrainingBatchSize = int.Parse(Console.ReadLine());
            Console.WriteLine("Begining Training:");
            myProg.TrainNetwork();
            Console.WriteLine("Training Finished.\nSaving Net.");
            myProg.SaveNetToFile();
            Console.WriteLine("Saved.");
            string mystring = Console.ReadLine();
        }

        public void SetupNet()
        {
            // if there is a pre-exsiting net layout use that. otherwise build a new net.
            if (!File.Exists("C:\\Users\\Tristan\\Desktop\\FinalYearProjectLearningGame\\Assets\\NeuralNetData\\NeuralNetDataLayout.txt"))
            {
                NetLayers = new NeuronLayer[NumLayers];

                for (int i = 0; i < NumLayers; ++i)
                {
                    NetLayers[i] = new NeuronLayer(LayerNeuronCounts[i], (i > 0) ? NetLayers[i - 1] : null);
                }

                Outputs = new float[LayerNeuronCounts[NumLayers - 1]];
                ExpectedOutputs = new float[LayerNeuronCounts[NumLayers - 1]];
            }
            else
            {
                LoadNetFromFile();
            }
        }

        public void FeedForward()
        {
            for (int i = 0; i < LayerNeuronCounts[0]; ++i)
            {
                NetLayers[0].Nodes[i].Activation = Inputs[i];
            }

            // Go through each layer that isn't the input layer.
            for (int i = 1; i < NumLayers; ++i)
            {
                // Go through each node in this layer and calculate it's activation value.
                for (int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    SigmoidNeuron tempNeuron = NetLayers[i].Nodes[j];
                    tempNeuron.Activation = Sigmoid(tempNeuron.GetZ());
                }
            }

            // Update the outputs from the output layer.
            for (int i = 0; i < LayerNeuronCounts[NumLayers - 1]; ++i)
            {
                Outputs[i] = NetLayers[NumLayers - 1].Nodes[i].Activation;
            }
        }

        public void Backprop()
        {
            FeedForward();

            // go through the output layer
            for (int i = 0; i < LayerNeuronCounts[NumLayers - 1]; ++i)
            {
                // for each neuron in the output layer calculate the difference between the output and the expected output
                SigmoidNeuron tempNeuron = NetLayers[NumLayers - 1].Nodes[i];
                tempNeuron.Delta = CostDerivertive(Outputs[i], ExpectedOutputs[i]) * SigmoidPrime(tempNeuron.z);
                tempNeuron.NablaBias += tempNeuron.Delta;

                // then go through each neuron connected to this neuron.
                for (int j = 0; j < tempNeuron.Inputs.Length; ++j)
                {
                    // set the change required in the weight.
                    tempNeuron.NablaWeights[j] += tempNeuron.Delta * tempNeuron.Inputs[j].Activation;
                    // and set it's required change property
                    tempNeuron.Inputs[j].Delta = (tempNeuron.Weights[j] * tempNeuron.Delta) * SigmoidPrime(tempNeuron.Inputs[j].z);
                }
            }

            // go through the remaning layers and nodes in those layers
            for (int i = NumLayers - 2; i > 0; --i)
            {
                for (int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    NetLayers[i].Nodes[j].NablaBias += NetLayers[i].Nodes[j].Delta;
                    // for each node connected to this node set the required change in weight and it's required change property.
                    for (int k = 0; k < NetLayers[i].Nodes[j].NablaWeights.Length; ++k)
                    {
                        NetLayers[i].Nodes[j].NablaWeights[k] += NetLayers[i].Nodes[j].Delta * NetLayers[i].Nodes[j].Inputs[k].Activation;
                        NetLayers[i].Nodes[j].Inputs[k].Delta = (NetLayers[i].Nodes[j].Weights[k] * NetLayers[i].Nodes[j].Delta) * SigmoidPrime(NetLayers[i].Nodes[j].Inputs[k].z);
                    }
                }
            }
        }

        void RunMiniBatch(DataSet[] batch)
        {
            resetNablas();

            for (int i = 0; i < batch.Length; ++i)
            {
                Inputs = batch[i].Inputs;
                ExpectedOutputs = batch[i].Outputs;
                Backprop();
            }

            for (int i = 1; i < NumLayers; ++i)
            {
                for (int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    for (int k = 0; k < NetLayers[i].Nodes[j].Weights.Length; ++k)
                    {
                        // for each node update it's weight and bias based on the changes calculated.
                        NetLayers[i].Nodes[j].Weights[k] = NetLayers[i].Nodes[j].Weights[k] - ((LearningRate / batch.Length) * NetLayers[i].Nodes[j].NablaWeights[k]);
                        NetLayers[i].Nodes[j].Bias = NetLayers[i].Nodes[j].Bias - ((LearningRate / batch.Length) * NetLayers[i].Nodes[j].NablaBias);
                    }
                }
            }
        }

        private void TrainNetwork()
        {
            // Get training data.
            DataSet[] TrainingData = GetTrainingData();

            for (int i = 0; i < NumTrainingCycles; ++i)
            {
                // Shuffle data
                TrainingData = ShuffleDataSet(TrainingData);
                // Create Mini Batches
                List<DataSet[]> Batches = ConvertDataSetToBatches(TrainingData);

                // loop through the mini batches
                for (int j = 0; j < Batches.Count; ++j)
                {
                    RunMiniBatch(Batches[j]);
                }

                // Test data stuff here.
                Console.WriteLine(string.Format("Training Cycle {0} / {1} Complete", (i + 1), NumTrainingCycles));
            }
        }

        List<DataSet[]> ConvertDataSetToBatches(DataSet[] dataSet)
        {
            List<DataSet[]> BatchList = new List<DataSet[]>();

            for (int i = 0; i < (int)Math.Ceiling((double)dataSet.Length / (double)TrainingBatchSize); ++i)
            {
                DataSet[] tempData = new DataSet[((i + 1) * TrainingBatchSize <= dataSet.Length) ? TrainingBatchSize : TrainingBatchSize - (((i + 1) * TrainingBatchSize) - dataSet.Length)];

                for (int j = 0; j < tempData.Length; ++j)
                {
                    tempData[j] = dataSet[(i * TrainingBatchSize) + j];
                }

                BatchList.Add(tempData);
            }

            return BatchList;
        }

        DataSet[] ShuffleDataSet(DataSet[] dataSet)
        {
            DataSet[] ShuffledData = new DataSet[dataSet.Length];
            List<DataSet> DataToShuffle = new List<DataSet>();
            DataToShuffle.AddRange(dataSet);

            while (DataToShuffle.Count > 0)
            {
                Random rand = new Random();
                int RemovePos = (int)Math.Round(rand.NextDouble() * (DataToShuffle.Count > 1 ? DataToShuffle.Count - 1 : 0));
                ShuffledData[dataSet.Length - DataToShuffle.Count] = DataToShuffle[RemovePos];
                DataToShuffle.RemoveAt(RemovePos);
            }

            return ShuffledData;
        }

        DataSet[] GetTrainingData()
        {
            string[] InputsString = File.ReadAllLines("C:\\Users\\Tristan\\Desktop\\FinalYearProjectLearningGame\\Assets\\Resources\\NeuralNetFiles\\TrainingInputs.txt");
            string[] OutputsString = File.ReadAllLines("C:\\Users\\Tristan\\Desktop\\FinalYearProjectLearningGame\\Assets\\Resources\\NeuralNetFiles\\TrainingOutputs.txt");

            if (InputsString.Length == OutputsString.Length)
            {

                DataSet[] TrainingData = new DataSet[InputsString.Length];

                for (int i = 0; i < TrainingData.Length; ++i)
                {
                    TrainingData[i] = new DataSet(InputsString[i].Split(','), OutputsString[i].Split(','));
                }
                return TrainingData;
            }

            return null;
        }

        void LoadNetFromFile()
        {
            string[] Neurons = File.ReadAllLines("C:\\Users\\Tristan\\Desktop\\FinalYearProjectLearningGame\\Assets\\NeuralNetData\\NeuralNetDataLayout.txt");

            NetLayers = new NeuronLayer[NumLayers];

            int StartCount = 0;

            for (int i = 0; i < NumLayers; ++i)
            {
                string[] LayerNeuronData = GetNeuronRange(StartCount, (i > 0) ? LayerNeuronCounts[i] + StartCount : 0, Neurons);

                NetLayers[i] = new NeuronLayer(LayerNeuronCounts[i], LayerNeuronData, (i > 0) ? NetLayers[i - 1] : null);

                StartCount += (i > 0) ? LayerNeuronCounts[i] : 0;
            }

            Outputs = new float[LayerNeuronCounts[NumLayers - 1]];
            ExpectedOutputs = new float[LayerNeuronCounts[NumLayers - 1]];
        }

        string[] GetNeuronRange(int Start, int End, string[] TotalData)
        {
            int length = End - Start;
            string[] ReturnData = new string[length];

            for (int i = Start; i < End; ++i)
            {
                ReturnData[i - Start] = TotalData[i];
            }

            return ReturnData;
        }

        public void SaveNetToFile()
        {
            string NetData = "";

            for (int i = 1; i < NumLayers; ++i)
            {
                for (int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    for (int k = 0; k < NetLayers[i].Nodes[j].Weights.Length; ++k)
                    {
                        NetData += NetLayers[i].Nodes[j].Weights[k].ToString();
                        NetData += (k != NetLayers[i].Nodes[j].Weights.Length - 1) ? "," : "";
                    }
                    NetData += "|";
                    NetData += NetLayers[i].Nodes[j].Bias.ToString();
                    NetData += (j != LayerNeuronCounts[i] - 1 || i != NumLayers - 1) ? "\n" : "";
                }
            }

            File.WriteAllText("C:\\Users\\Tristan\\Desktop\\FinalYearProjectLearningGame\\Assets\\NeuralNetData\\NeuralNetDataLayout.txt", NetData);
        }

        // if we are going for batches.
        void resetNablas()
        {
            for (int i = 1; i < NumLayers; ++i)
            {
                for (int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    NetLayers[i].Nodes[j].ResetNablas();
                }
            }
        }

        float CostDerivertive(float Output, float ExpectedOutput)
        {
            return Output - ExpectedOutput;
        }

        float Sigmoid(float z)
        {
            return 1 / (1 + (float)Math.Exp(-z));
        }

        float SigmoidPrime(float z)
        {
            float Sig = Sigmoid(z);
            return Sig * (1 - Sig);
        }

        class DataSet
        {
            public float[] Inputs;
            public float[] Outputs;

            public DataSet(string[] Ins, string[] Outs)
            {
                Inputs = new float[Ins.Length];
                Outputs = new float[Outs.Length];

                for (int i = 0; i < Ins.Length; ++i)
                {
                    Inputs[i] = float.Parse(Ins[i]);
                }

                for (int i = 0; i < Outs.Length; ++i)
                {
                    Outputs[i] = float.Parse(Outs[i]);
                }
            }

        }

    }
}
