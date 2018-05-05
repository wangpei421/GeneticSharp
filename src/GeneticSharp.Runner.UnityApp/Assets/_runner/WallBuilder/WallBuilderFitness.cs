using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Linq;

namespace GeneticSharp.Runner.UnityApp.WallBuilder
{
    public class WallBuilderFitness : IFitness
    {
        private float m_secondsForEvaluation = 5;

        public WallBuilderFitness(float secondsForEvaluation)
        {
            m_secondsForEvaluation = secondsForEvaluation;
            ChromosomesToBeginEvaluation = new BlockingCollection<WallBuilderChromosome>();
            ChromosomesToEndEvaluation = new BlockingCollection<WallBuilderChromosome>();
        }

        public BlockingCollection<WallBuilderChromosome> ChromosomesToBeginEvaluation { get; private set; }
        public BlockingCollection<WallBuilderChromosome> ChromosomesToEndEvaluation { get; private set; }

        public double Evaluate(IChromosome chromosome)
        {
            var c = chromosome as WallBuilderChromosome;
            ChromosomesToBeginEvaluation.Add(c);
            Thread.Sleep(TimeSpan.FromSeconds(m_secondsForEvaluation));

            ChromosomesToEndEvaluation.Add(c);

            do
            {
                Thread.Sleep(100);
            } while (!c.Evaluated);

            return c.FloorHits * -1;
        }

    }
}