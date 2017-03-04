using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNet
{
    [System.Serializable]
    public class NeuralNetNode
    {
        public float[] Weight;
        public float[] Bias;
        public float Input;


        public NeuralNetNode(int NextLayerNodeCount)
        {
            Weight = new float[NextLayerNodeCount];
            Bias = new float[NextLayerNodeCount];

            for(int i = 0; i < NextLayerNodeCount; ++i)
            {
                Weight[i] = Random.value;
                Bias[i] = Random.value;
            }
        }

    }

}