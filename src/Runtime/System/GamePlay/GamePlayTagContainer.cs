// 用于封装GamePlayTag集合，接口和行为与Lua的GamePlayTagContainer保持一致
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CPL
{
    [Serializable]
    public class GamePlayTagContainer
    {
        // parentTags在PrefabConfig的导出中不会被序列化，但是在编辑器中会存在.prefab里
        [SerializeField]private List<string> gamePlayTags = new();
        public List<string> GamePlayTags => gamePlayTags;
        [SerializeField]private List<string> parentTags = new();
        public List<string> ParentTags => parentTags;
        
        private const string Explicit = "Explicit";
        private const string IncludeParentTags = "IncludeParentTags";

        public GamePlayTagContainer()
        {
            
        }
        
        public GamePlayTagContainer(List<string> tags)
        {
            foreach (var tag in tags)
            {
                if(!gamePlayTags.Contains(tag))
                    gamePlayTags.Add(tag);
            }
            FillParentTags();
        }
        
        public static GamePlayTagContainer CreateFromArray(List<string> gamePlayTags)
        {
            return new GamePlayTagContainer(gamePlayTags);
        }
        
        public static bool IsValid(string tag)
        {
            return !string.IsNullOrEmpty(tag);
        }

        public GamePlayTagContainer AddTag(string tagToAdd)
        {
            if(!IsValid(tagToAdd)) return null;
            if (!gamePlayTags.Contains(tagToAdd))
                gamePlayTags.Add(tagToAdd);
            ExtractParentTags(tagToAdd, parentTags);
            return this;
        }
        
        public GamePlayTagContainer AddTagFast(string tagToAdd)
        {
            if(!IsValid(tagToAdd)) return null;
            gamePlayTags.Add(tagToAdd);
            ExtractParentTags(tagToAdd, parentTags);
            return this;
        }

        public GamePlayTagContainer RemoveTag(string tagToRemove)
        {
            if(!IsValid(tagToRemove)) return null;
            int count = gamePlayTags.RemoveAll(tag => tag == tagToRemove);
            if(count > 0)
            {
                FillParentTags();
            }
            return this;
        }

        public void FillParentTags()
        {
            parentTags.Clear();
            foreach (var tag in gamePlayTags)
            {
                ExtractParentTags(tag, parentTags);
            }
        }

        public static List<string> ExtractParentTags(string gamePlayTag, List<string> parentTags)
        {
            var tags = gamePlayTag.Split('.');
            if(tags == null || tags.Length == 0) return null;
            for (int i = 0; i < tags.Length - 1; i++)
            {
                var parentTag = string.Join(".", tags, 0, i + 1);
                if (!parentTags.Contains(parentTag))
                    parentTags.Add(parentTag);
            }
            return parentTags;
        }
        
        public bool HasTag(string tagToCheck)
        {
            if(!IsValid(tagToCheck)) return false;
            return gamePlayTags.Contains(tagToCheck) || parentTags.Contains(tagToCheck);
        }

        public bool HasTagExact(string tagToCheck)
        {
            if (!IsValid(tagToCheck)) return false;
            return gamePlayTags.Contains(tagToCheck);
        }

        public bool HasAny(List<string> tagList)
        {
            foreach (var tag in tagList)
            {
                if(HasTag(tag)) return true;
            }
            return false;
        }
        
        public bool HasAnyExact(List<string> tagList)
        {
            foreach (var tag in tagList)
            {
                if(HasTagExact(tag)) return true;
            }
            return false;
        }
        
        public bool HasAll(List<string> tagList)
        {
            foreach (var tag in tagList)
            {
                if(!HasTag(tag)) return false;
            }
            return true;
        }
        
        public bool HasAllExact(List<string> tagList)
        {
            foreach (var tag in tagList)
            {
                if(!HasTagExact(tag)) return false;
            }
            return true;
        }
        
        public bool ComplexHasTag(string tagToCheck, string tagMatchType,string tagToCheckMatchType)
        {
            Assert.IsFalse(tagMatchType == Explicit || tagMatchType == IncludeParentTags,
                "Both match types cannot be Explicit");
            if (tagMatchType == IncludeParentTags)
            {
                var expendedContainer = GetGamePlayTagParents();
                return expendedContainer.HasTagFast(tagToCheck, Explicit, tagToCheckMatchType);
            }else
                return GetSingleTagContainer(tagToCheck).DoesTagContainerMatch(this, IncludeParentTags, Explicit, "Any");
        }

        public GamePlayTagContainer GetGamePlayTagParents()
        {
            var resultContainer = new GamePlayTagContainer();
            resultContainer.gamePlayTags = new List<string>(gamePlayTags);
            
            foreach (var tag in parentTags)
            {
                if(!resultContainer.gamePlayTags.Contains(tag))
                    resultContainer.gamePlayTags.Add(tag);
            }
            return resultContainer;
        }

        public static GamePlayTagContainer GetSingleTagContainer(string tag)
        {
            var container = new GamePlayTagContainer();
            container.AddTag(tag);
            return container;
        }

        public bool HasTagFast(string tagToCheck, string tagMatchType, string tagToCheckMatchType)
        {
            bool result;
            if (tagToCheckMatchType == Explicit)
            {
                result = gamePlayTags.Contains(tagToCheck);
                if (!result && tagMatchType == IncludeParentTags)
                {
                    result = parentTags.Contains(tagToCheck);
                }
            }else
                result = ComplexHasTag(tagToCheck, tagMatchType, tagToCheckMatchType);
            return result;
        }

        public bool DoesTagContainerMatch(GamePlayTagContainer otherContainer, string tagMatchType, string otherTagMatchType,
            string containerMatchType)
        {
            bool result;
            if (otherTagMatchType == Explicit)
            {
                result = containerMatchType == "All";
                foreach (var otherTag in otherContainer.gamePlayTags)
                {
                    if (HasTagFast(otherTag, tagMatchType, otherTagMatchType))
                    {
                        if (containerMatchType == "Any")
                        {
                            result = true;
                            break;
                        }
                    }else
                    {
                        if (containerMatchType == "All")
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }else
            {
                var otherExpanded = otherContainer.GetGamePlayTagParents();
                return DoesTagContainerMatch(otherExpanded, tagMatchType, Explicit, containerMatchType);
            }
            return result;
        }
        
        public override string ToString()
        {
            return string.Join('|', gamePlayTags);
        }

        public string ToDebugString()
        {
            string gamePlayTagStr = string.Join('|', this.gamePlayTags);
            string parentTagStr = string.Join('|', this.parentTags);
            return $"GamePlayTags: {gamePlayTagStr}\n, ParentTags: {parentTagStr}";
        }
    }
}