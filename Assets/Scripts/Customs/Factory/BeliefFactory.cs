using System;
using System.Collections.Generic;
using GOAP;
using UnityEngine;

namespace CharacterOrEnemyEffect.Factory
{
    public class BeliefFactory
    {
        //private readonly GoapAgent _agent;
        private readonly Dictionary<string, AgentBelief> _beliefs;

        public BeliefFactory(Dictionary<string, AgentBelief> beliefs)
        {
            //_agent = agent;
            _beliefs = beliefs;
        }

        public void AddBeliefCondition(string key, Func<bool> condition)
        {
            var belief = new BeliefBuilder(key);
            
            _beliefs.Add(key,
            belief
                .WithCondition(condition)
                .BuildBelief());
        }
        
        public void AddLocationBelief(string key, Vector3 locationCondition, Func<bool> condition)
        {
            var belief = new BeliefBuilder(key);
            _beliefs.Add(key, 
                belief
                .WithCondition(condition)
                .WithLocation(() => locationCondition)
                .BuildBelief());
        }

        public void AddSensorBelief(string key, ISensor sensor)
        {
            var belief = new BeliefBuilder(key);
            
            _beliefs.Add(key, 
                belief
                .WithCondition(() => sensor.IsTargetInSensor)
                .WithLocation(() => sensor.Target == null ? Vector3.zero : sensor.Target.Value)
                .BuildBelief());
        }

        //bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(_agent, pos) < range; ///СОМНИТЕЛЬНО, ПОЗЖЕ ПЕРЕДЕЛАТЬ
    }
}