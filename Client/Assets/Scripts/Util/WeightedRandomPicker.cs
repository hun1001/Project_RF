using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Util
{
    /// <summary> 가중치 랜덤 뽑기 </summary>
    public class WeightedRandomPicker<T>
    {
        /// <summary> 전체 아이템의 가중치 합 </summary>
        public double SumOfWeights
        {
            get
            {
                CalculateSumIfDirty();
                return _sumOfWeights;
            }
        }

        private System.Random _randomInstance;
        private readonly Dictionary<T, double> _itemWeightDict;
        private readonly Dictionary<T, double> _normalizedItemWeightDict; // 확률이 정규화된 아이템 목록

        /// <summary> 가중치 합이 계산되지 않은 상태인지 여부 </summary>
        private bool _isDirty;
        private double _sumOfWeights;

        /***********************************************************************
        *                               Constructors
        ***********************************************************************/
        #region .
        public WeightedRandomPicker()
        {
            _randomInstance = new System.Random();
            _itemWeightDict = new Dictionary<T, double>();
            _normalizedItemWeightDict = new Dictionary<T, double>();
            _isDirty = true;
            _sumOfWeights = 0.0;
        }

        public WeightedRandomPicker(int randomSeed)
        {
            _randomInstance = new System.Random(randomSeed);
            _itemWeightDict = new Dictionary<T, double>();
            _normalizedItemWeightDict = new Dictionary<T, double>();
            _isDirty = true;
            _sumOfWeights = 0.0;
        }

        #endregion
        /***********************************************************************
        *                               Add Methods
        ***********************************************************************/
        #region .

        /// <summary> 새로운 아이템-가중치 쌍 추가 </summary>
        public void Add(T item, double weight)
        {
            CheckDuplicatedItem(item);
            CheckValidWeight(weight);

            _itemWeightDict.Add(item, weight);
            _isDirty = true;
        }

        /// <summary> 새로운 아이템-가중치 쌍들 추가 </summary>
        public void Add(params (T item, double weight)[] pairs)
        {
            foreach (var pair in pairs)
            {
                CheckDuplicatedItem(pair.item);
                CheckValidWeight(pair.weight);

                _itemWeightDict.Add(pair.item, pair.weight);
            }
            _isDirty = true;
        }

        #endregion
        /***********************************************************************
        *                               Public Methods
        ***********************************************************************/
        #region .

        /// <summary> 목록에서 대상 아이템 제거 </summary>
        public void Remove(T item)
        {
            CheckNotExistedItem(item);

            _itemWeightDict.Remove(item);
            _isDirty = true;
        }

        public void Clear()
        {
            _itemWeightDict.Clear();
            _normalizedItemWeightDict.Clear();
            _isDirty = true;
            _sumOfWeights = 0;
            _randomInstance = null;
            GC.Collect();
            _randomInstance = new System.Random();
        }

        /// <summary> 대상 아이템의 가중치 수정 </summary>
        public void ModifyWeight(T item, double weight)
        {
            CheckNotExistedItem(item);
            CheckValidWeight(weight);

            _itemWeightDict[item] = weight;
            _isDirty = true;
        }

        /// <summary> 랜덤 시드 재설정 </summary>
        public void ReSeed(int seed)
        {
            _randomInstance = new System.Random(seed);
        }

        public int GetLength()
        {
            return _itemWeightDict.Count;
        }

        #endregion
        /***********************************************************************
        *                               Getter Methods
        ***********************************************************************/
        #region .

        /// <summary> 랜덤 뽑기 </summary>
        public T GetRandomPick()
        {
            // 랜덤 계산
            double chance = _randomInstance.NextDouble(); // [0.0, 1.0)
            chance *= SumOfWeights;

            return GetRandomPick(chance);
        }

        /// <summary> 직접 랜덤 값을 지정하여 뽑기 </summary>
        public T GetRandomPick(double randomValue)
        {
            if (randomValue < 0.0) randomValue = 0.0;
            if (randomValue > SumOfWeights) randomValue = SumOfWeights - 0.00000001;

            double current = 0.0;
            foreach (var pair in _itemWeightDict)
            {
                current += pair.Value;

                if (randomValue < current)
                {
                    return pair.Key;
                }
            }

            throw new Exception($"Unreachable - [Random Value : {randomValue}, Current Value : {current}]");
            //return itemPairList[itemPairList.Count - 1].item; // Last Item
        }

        /// <summary> 대상 아이템의 가중치 확인 </summary>
        public double GetWeight(T item)
        {
            return _itemWeightDict[item];
        }

        /// <summary> 대상 아이템의 정규화된 가중치 확인 </summary>
        public double GetNormalizedWeight(T item)
        {
            CalculateSumIfDirty();
            return _normalizedItemWeightDict[item];
        }

        /// <summary> 아이템 목록 확인(읽기 전용) </summary>
        public ReadOnlyDictionary<T, double> GetItemDictReadonly()
        {
            return new ReadOnlyDictionary<T, double>(_itemWeightDict);
        }

        /// <summary> 가중치 합이 1이 되도록 정규화된 아이템 목록 확인(읽기 전용) </summary>
        public ReadOnlyDictionary<T, double> GetNormalizedItemDictReadonly()
        {
            CalculateSumIfDirty();
            return new ReadOnlyDictionary<T, double>(_normalizedItemWeightDict);
        }

        #endregion
        /***********************************************************************
        *                               Private Methods
        ***********************************************************************/
        #region .

        /// <summary> 모든 아이템의 가중치 합 계산해놓기 </summary>
        private void CalculateSumIfDirty()
        {
            if (!_isDirty) return;
            _isDirty = false;

            _sumOfWeights = 0.0;
            foreach (var pair in _itemWeightDict)
            {
                _sumOfWeights += pair.Value;
            }

            // 정규화 딕셔너리도 업데이트
            UpdateNormalizedDict();
        }

        /// <summary> 정규화된 딕셔너리 업데이트 </summary>
        private void UpdateNormalizedDict()
        {
            _normalizedItemWeightDict.Clear();
            foreach (var pair in _itemWeightDict)
            {
                _normalizedItemWeightDict.Add(pair.Key, pair.Value / _sumOfWeights);
            }
        }

        /// <summary> 이미 아이템이 존재하는지 여부 검사 </summary>
        private void CheckDuplicatedItem(T item)
        {
            if (_itemWeightDict.ContainsKey(item))
                throw new Exception($"이미 [{item}] 아이템이 존재합니다.");
        }

        /// <summary> 존재하지 않는 아이템인 경우 </summary>
        private void CheckNotExistedItem(T item)
        {
            if (!_itemWeightDict.ContainsKey(item))
                throw new Exception($"[{item}] 아이템이 목록에 존재하지 않습니다.");
        }

        /// <summary> 가중치 값 범위 검사(0보다 커야 함) </summary>
        private void CheckValidWeight(in double weight)
        {
            if (weight <= 0f)
                throw new Exception("가중치 값은 0보다 커야 합니다.");
        }

        #endregion
    }
}
