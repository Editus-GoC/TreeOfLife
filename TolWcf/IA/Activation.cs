using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;
using Accord.Neuro.Learning;
using AForge.Neuro;
using Tol.Common;
using Tol.Dal;

namespace Tol.IA
{
    public class Activation
    {
        private readonly List<List<double>> _inputs;
        private readonly List<int> _outputs;
        private readonly IActivationFunction _activationFunction;
        private readonly double[] _result;
        public Activation(double[] result)
        {
            _activationFunction = new BipolarSigmoidFunction();
            _inputs = new List<List<double>>();
            _outputs = new List<int>();
            _result = result;
        }

        public double[][] Inputs
        {
            get { return _inputs.Select(s => s.ToArray()).ToArray(); }
            set { }
        }

        public int[] Outputs
        {
            get { return _outputs.ToArray(); }
            set { }
        }

        //calcul ai for all profil with stat data
        public void ComputeIA()
        {
            int defaultTime = 0;
            using (var tol = new TOLEntities())
            {
                foreach (var profil in tol.PROFIL.Where(w => w.ID_PROFIL == 1 || w.ID_PROFIL == 3))
                {
                    ComputeStat(profil.ID_PROFIL);

                    var network = Compute();

                    //now ours network is train we can test
                    if (network != null)
                    {
                        //we want answer for good result
                        var compute = network.Compute(_result);
                        //we save current neural network result
                        if (defaultTime == 0)
                        {
                            defaultTime = CreateDefaultTime();
                        }
                        SaveCompute(profil.ID_PROFIL, compute, defaultTime);
                    }
                    else
                    {
                        if (tol.BUBBLE.Count(a => a.ID_PROFIL == profil.ID_PROFIL) == 0)
                        {
                            if (defaultTime == 0)
                            {
                                defaultTime = CreateDefaultTime();
                            }
                            CreateDefaultBubble(profil.ID_PROFIL, defaultTime);
                        }
                    }
                }
            }
        }

        private void SaveCompute(int idProfil, double[] compute, int idTime)
        {
            using (var tol = new TOLEntities())
            {
                for (int i = 0; i < compute.Length-1; i++)
                {
                    var bubble = new BUBBLE();
                    bubble.SIZE = (decimal)compute[i];
                    bubble.ID_FILTER = Outputs[i];
                    bubble.ID_TIME = idTime;
                    bubble.ID_PROFIL = idProfil;
                    if (tol.FILTER.Count(c => c.ID_FILTER == bubble.ID_FILTER) != 0)
                        tol.BUBBLE.Add(bubble);
                }
                tol.SaveChanges();
            }
        }

        private int CreateDefaultTime()
        {
            var time = new TIME();
            using (var tol = new TOLEntities())
            {

                time.TIME1 = DateTime.Now;
                tol.TIME.Add(time);

                tol.SaveChanges();
            }
            return time.ID_TIME;
        }

        private void CreateDefaultBubble(int idProfil, int idTime)
        {
            using (var tol = new TOLEntities())
            {
                foreach (var filter in tol.FILTER.Where(w => w.ACTIF.Value))
                {
                    var bubble = new BUBBLE();
                    bubble.SIZE = -1;
                    bubble.ID_FILTER = filter.ID_FILTER;
                    bubble.ID_TIME = idTime;
                    bubble.ID_PROFIL = idProfil;
                    tol.BUBBLE.Add(bubble);
                }

                tol.SaveChanges();
            }
        }

        //Create inputs and outputs value
        private void ComputeStat(int idProfil)
        {
            using (var tol = new TOLEntities())
            {
                foreach (var filter in tol.FILTER.Where(w => w.ACTIF.Value))
                {
                    var input = new List<double>();
                    input.Add(tol.STAT.Count(w => w.ID_PROFIL == idProfil && w.CHOOSED) == 0 ? 1 : -1);
                    _inputs.Add(input);
                    _outputs.Add(filter.ID_FILTER);
                }
            }
        }

        public static double[][] Expand(int[] labels, int classes, double negative, double positive)
        {
            double[][] numArray1 = new double[labels.Length][];
            for (int index1 = 0; index1 < labels.Length; ++index1)
            {
                double[] numArray2 = numArray1[index1] = new double[classes];
                for (int index2 = 0; index2 < numArray2.Length; ++index2)
                    numArray2[index2] = negative;
                try
                {
                    numArray2[labels[index1]] = positive;
                }
                catch (Exception)
                {
                }

            }
            return numArray1;
        }

        //Train neural network
        private ActivationNetwork Compute()
        {
            if (Inputs.Length == 0 || Outputs.Length == 0)
                return null;

            var numberOfPossibleValue = Outputs.Distinct().Length + 1;

            // In our problem, we have 2 inputs (x, y pairs), and we will 
            // be creating a network with 5 hidden neurons and 7 possibility:
            var network = new ActivationNetwork(_activationFunction, Inputs[0].Length, Const.NumberOfNeuron, numberOfPossibleValue);

            // Create a Levenberg-Marquardt algorithm
            var teacher = new LevenbergMarquardtLearning(network)
            {
                UseRegularization = true
            };

            // Because the network is expecting multiple outputs,
            // we have to convert our single variable into arrays

            var y = Expand(Outputs, numberOfPossibleValue, -1, 1);

            // Iterate until stop criteria is met
            double error = double.PositiveInfinity;
            double previous;

            //compute while error is enought good
            do
            {
                previous = error;
                error = teacher.RunEpoch(Inputs, y);

            } while (Math.Abs(previous - error) > Const.NeuralTrainError);

            return network;
        }
    }
}
