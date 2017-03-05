using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NeuralNet
{

    public class NeuralNetController : MonoBehaviour
    {
        public static NeuralNetController NNC;

        public float[] Inputs;
        public float[] Outputs;
        public float[] ExpectedOutputs;

        public TextAsset LearningInputs;
        public TextAsset LearningOutputs;

        public int NumTrainingCycles;
        public int TrainingBatchSize;
        public float LearningRate;
        public int NumLayers;
        public int[] LayerNeuronCounts;
        NeuronLayer[] NetLayers;

        public float TargetFrameRate;

        float FrameStartTime;

        float CurrentFrameTime
        {
            get
            {
                return Time.realtimeSinceStartup - FrameStartTime;
            }
        }

        void Start()
        {
            NNC = this;
            SetupNet();
        }

        void Update()
        {
            FrameStartTime = Time.realtimeSinceStartup;
        }

        void SetupNet()
        {
            NetLayers = new NeuronLayer[NumLayers];

            for (int i = 0; i < NumLayers; ++i)
            {
                NetLayers[i] = new NeuronLayer(LayerNeuronCounts[i], (i > 0) ? NetLayers[i - 1] : null);
            }

            Outputs = new float[LayerNeuronCounts[NumLayers - 1]];
        }

        public void RunFeedForward()
        {
            StartCoroutine(FeedForward());
        }

        // Might want to move into a co-routine as this might become very large and expensive.
        private IEnumerator FeedForward()
        {
            yield return new WaitForEndOfFrame();
            // Update the Input layer with updated inputs.
            for (int i = 0; i < LayerNeuronCounts[0]; ++i)
            {
                NetLayers[0].Nodes[i].Activation = Inputs[i];

                if (ShouldYield())
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            // Go through each layer that isn't the input layer.
            for (int i = 1; i < NumLayers; ++i)
            {
                // Go through each node in this layer and calculate it's activation value.
                for (int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    SigmoidNeuron tempNeuron = NetLayers[i].Nodes[j];
                    tempNeuron.Activation = Sigmoid(tempNeuron.GetZ());

                    if (ShouldYield())
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }

            // Update the outputs from the output layer.
            for (int i = 0; i < LayerNeuronCounts[NumLayers - 1]; ++i)
            {
                Outputs[i] = NetLayers[NumLayers - 1].Nodes[i].Activation;

                if (ShouldYield())
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public IEnumerator Backprop()
        {
            yield return new WaitForEndOfFrame();
            // feed forward
            yield return FeedForward();

            if (ShouldYield())
            {
                yield return new WaitForEndOfFrame();
            }

            // go through the output layer
            for (int i = 0; i < LayerNeuronCounts[NumLayers - 1]; ++i)
            {
                // for each neuron in the output layer calculate the difference between the output and the expected output
                SigmoidNeuron tempNeuron = NetLayers[NumLayers - 1].Nodes[i];
                tempNeuron.Delta = CostDerivertive(Outputs[i], ExpectedOutputs[i]) * SigmoidPrime(tempNeuron.z);
                tempNeuron.NablaBias += tempNeuron.Delta;

                // then go through each neuron connected to this neuron.
                for(int j = 0; j < tempNeuron.Inputs.Length; ++j)
                {
                    // set the change required in the weight.
                    tempNeuron.NablaWeights[j] += tempNeuron.Delta * tempNeuron.Inputs[j].Activation;
                    // and set it's required change property
                    tempNeuron.Inputs[j].Delta = (tempNeuron.Weights[j] * tempNeuron.Delta) * SigmoidPrime(tempNeuron.Inputs[j].z);

                    if (ShouldYield())
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }

            // go through the remaning layers and nodes in those layers
            for(int i = NumLayers - 2; i > 0; --i)
            {
                for(int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    NetLayers[i].Nodes[j].NablaBias += NetLayers[i].Nodes[j].Delta;
                    // for each node connected to this node set the required change in weight and it's required change property.
                    for (int k = 0; k < NetLayers[i].Nodes[j].NablaWeights.Length; ++k)
                    {
                        NetLayers[i].Nodes[j].NablaWeights[k] += NetLayers[i].Nodes[j].Delta * NetLayers[i].Nodes[j].Inputs[k].Activation;
                        NetLayers[i].Nodes[j].Inputs[k].Delta = (NetLayers[i].Nodes[j].Weights[k] * NetLayers[i].Nodes[j].Delta) * SigmoidPrime(NetLayers[i].Nodes[j].Inputs[k].z);

                        if (ShouldYield())
                        {
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
            }
        }

        IEnumerator RunMiniBatch(DataSet[] batch)
        {
            yield return new WaitForEndOfFrame();
            resetNablas();

            if (ShouldYield())
            {
                yield return new WaitForEndOfFrame();
            }

            for(int i = 0; i < batch.Length; ++i)
            {
                Inputs = batch[i].Inputs;
                ExpectedOutputs = batch[i].Outputs;
                yield return Backprop();

                if (ShouldYield())
                {
                    yield return new WaitForEndOfFrame();
                }
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

                        if (ShouldYield())
                        {
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
            }
        }

        public void RunTrainNetwork()
        {
            StartCoroutine(TrainNetwork());
        }

        private IEnumerator TrainNetwork()
        {
            yield return new WaitForEndOfFrame();
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
                    yield return RunMiniBatch(Batches[j]);
                }

                // Test data stuff here.
                print("Training Cycle " + (i + 1) + "/" + NumTrainingCycles + " Complete");

                if (ShouldYield())
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        List<DataSet[]> ConvertDataSetToBatches(DataSet[] dataSet)
        {
            List<DataSet[]> BatchList = new List<DataSet[]>();

            for(int i = 0; i < Mathf.CeilToInt((float)dataSet.Length / (float)TrainingBatchSize); ++i)
            {
                DataSet[] tempData = new DataSet[((i + 1)*TrainingBatchSize <= dataSet.Length) ? TrainingBatchSize : TrainingBatchSize - (((i + 1) * TrainingBatchSize) - dataSet.Length)];

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
                int RemovePos = Random.Range(0, DataToShuffle.Count);
                ShuffledData[dataSet.Length - DataToShuffle.Count] = DataToShuffle[RemovePos];
                DataToShuffle.RemoveAt(RemovePos);
            }

            return ShuffledData;
        }

        DataSet[] GetTrainingData()
        {
            string[] InputsString = LearningInputs.text.Split('\n');
            string[] OutputsString = LearningOutputs.text.Split('\n');

            if (InputsString.Length == OutputsString.Length)
            {

                DataSet[] TrainingData = new DataSet[InputsString.Length];

                for (int i = 0; i < TrainingData.Length; ++i)
                {
                    TrainingData[i] = new DataSet(InputsString[i].Split(','), OutputsString[i].Split(','));
                }
                return TrainingData;
            }
            else
            {
                Debug.LogError(string.Format("Training Data quantity mis-match: Input Quantity {0} | Output Quantity {1}", InputsString.Length, OutputsString.Length));
            }

            return null;
        }

        bool ShouldYield()
        {
            return CurrentFrameTime > 1f / TargetFrameRate;
        }

        // if we are going for batches.
        void resetNablas()
        {
            for(int i = 1; i < NumLayers; ++i)
            {
                for(int j = 0; j < LayerNeuronCounts[i]; ++j)
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
            return 1 / (1 + Mathf.Exp(-z));
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

                for(int i = 0; i < Ins.Length; ++i)
                {
                    Inputs[i] = float.Parse(Ins[i]);
                    Outputs[i] = float.Parse(Outs[i]);
                }
            }

        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(NeuralNetController))]
    public class NeuralNetEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Begin Learning"))
            {
                ((NeuralNetController)target).RunTrainNetwork();
            }

            if (GUILayout.Button("Feed Forward"))
            {
                ((NeuralNetController)target).RunFeedForward();
            }
        }

    }
#endif
}
