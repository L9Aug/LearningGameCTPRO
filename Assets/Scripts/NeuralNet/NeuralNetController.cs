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

        public float[] Input;
        public float[] Output;

        public int NumLayers;
        public int[] LayerNeuronCounts;
        public NeuralNetLayer[] NetLayers;



        void Start()
        {
            NNC = this;
            SetupNet();
        }

        void SetupNet()
        {
            NetLayers = new NeuralNetLayer[NumLayers];
            for (int i = 0; i < NumLayers; ++i)
            {
                NetLayers[i] = new NeuralNetLayer(LayerNeuronCounts[i], (i != NumLayers - 1) ? LayerNeuronCounts[i + 1] : 0);
            }
            //Input = new float[LayerNeuronCounts[0]];
            Output = new float[LayerNeuronCounts[NumLayers - 1]];
        }

        public void ForwardPass()
        {
            for (int i = 0; i < LayerNeuronCounts[0]; ++i)
            {
                NetLayers[0].Nodes[i].Input = Input[i];
                print("Input: " + Input[i]);
            }

            // Layer
            for (int i = 1; i < NumLayers; ++i)
            {
                // Node in this layer
                for (int j = 0; j < LayerNeuronCounts[i]; ++j)
                {
                    NetLayers[i].Nodes[j].Input = 0;
                    // Node in previous Layer
                    for (int k = 0; k < LayerNeuronCounts[i - 1]; ++k)
                    {
                        NeuralNetNode tempNode = NetLayers[i - 1].Nodes[k];
                        NetLayers[i].Nodes[j].Input += Sigmoid((tempNode.Weight[j] * tempNode.Input) + tempNode.Bias[j]);
                        print(NetLayers[i].Nodes[j].Input);
                    }
                }
            }

            for(int i = 0; i < LayerNeuronCounts[NumLayers - 1]; ++i)
            {
                Output[i] = NetLayers[NumLayers - 1].Nodes[i].Input;
                print("Output: " + Output[i]);
            }

        }

        float Sigmoid(float z)
        {
            return 1 / (1 + Mathf.Exp(-z));
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(NeuralNetController))]
    public class NeuralNetEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Run Forwards"))
            {
                ((NeuralNetController)target).ForwardPass();
            }
        }

    }
#endif
}
