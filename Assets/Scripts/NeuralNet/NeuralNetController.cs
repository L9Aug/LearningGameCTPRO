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
        public float[] ExpectedOutput;

        public float LearningRate;
        public int NumLayers;
        public int[] LayerNeuronCounts;
        public NeuronLayer[] NetLayers;

        void Start()
        {
            NNC = this;
            SetupNet();
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

        // Might want to move into a co-routine as this might become very large and expensive.
        public void FeedForward()
        {
            // Update the Input layer with updated inputs.
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
            // feed forward
            FeedForward();

            // go through the output layer
            for(int i = 0; i < LayerNeuronCounts[NumLayers - 1]; ++i)
            {
                // for each neuron in the output layer calculate the difference between the output and the expected output
                SigmoidNeuron tempNeuron = NetLayers[NumLayers - 1].Nodes[i];
                tempNeuron.Delta = CostDerivertive(Outputs[i], ExpectedOutput[i]) * SigmoidPrime(tempNeuron.z);
                tempNeuron.NablaBias = tempNeuron.Delta;

                // then go through each neuron connected to this neuron.
                for(int j = 0; j < tempNeuron.Inputs.Length; ++j)
                {
                    // set the change required in the weight.
                    tempNeuron.NablaWeights[j] = tempNeuron.Delta * tempNeuron.Inputs[j].Activation;
                    // and set it's required change property
                    tempNeuron.Inputs[j].Delta = (tempNeuron.Weights[j] * tempNeuron.Delta) * SigmoidPrime(tempNeuron.Inputs[j].z);
                }
            }

            // go through the remaning layers and nodes in those layers
            for(int i = NumLayers - 2; i > 0; --i)
            {
                for(int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    NetLayers[i].Nodes[j].NablaBias = NetLayers[i].Nodes[j].Delta;
                    // for each node connected to this node set the required change in weight and it's required change property.
                    for (int k = 0; k < NetLayers[i].Nodes[j].NablaWeights.Length; ++k)
                    {
                        NetLayers[i].Nodes[j].NablaWeights[k] = NetLayers[i].Nodes[j].Delta * NetLayers[i].Nodes[j].Inputs[k].Activation;
                        NetLayers[i].Nodes[j].Inputs[k].Delta = (NetLayers[i].Nodes[j].Weights[k] * NetLayers[i].Nodes[j].Delta) * SigmoidPrime(NetLayers[i].Nodes[j].Inputs[k].z);
                    }
                }
            }

            // is seperate as the nablas are cumulative for multiple test samples at one pass, and changes the LearningRate to Learning rate / num samples.
            for(int i = 1; i < NumLayers; ++i)
            {
                for(int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    for(int k = 0; k < NetLayers[i].Nodes[j].Weights.Length; ++k)
                    {
                        // for each node update it's weight and bias based on the changes calculated.
                        NetLayers[i].Nodes[j].Weights[k] = NetLayers[i].Nodes[j].Weights[k] - (LearningRate * NetLayers[i].Nodes[j].NablaWeights[k]);
                        NetLayers[i].Nodes[j].Bias = NetLayers[i].Nodes[j].Bias - (LearningRate * NetLayers[i].Nodes[j].NablaBias);
                    }                    
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

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(NeuralNetController))]
    public class NeuralNetEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Test LEarning"))
            {
                ((NeuralNetController)target).Backprop();
            }
        }

    }
#endif
}
