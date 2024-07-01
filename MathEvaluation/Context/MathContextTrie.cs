using System;
using System.Collections.Concurrent;

namespace MathEvaluation.Context;

/// <summary>
/// A prefix tree, also known as a trie (pronounced "try"), is a type of search tree. 
/// It is a special form of a digital tree that represents a dynamic set of strings, 
/// usually words in a dictionary, in a way that allows for efficient retrieval.
/// </summary>
internal sealed class MathContextTrie
{
    private readonly TrieNode _rootNode = new();

    public void AddMathEntity(IMathEntity entity)
    {
        AddMathEntity(_rootNode, entity.Key.AsSpan(), entity);
    }

    public IMathEntity? FindMathEntity(ReadOnlySpan<char> expression)
    {
        return FindMathEntity(_rootNode, expression);
    }

    private void AddMathEntity(TrieNode trieNode, ReadOnlySpan<char> key, IMathEntity entity)
    {
        if (key.IsEmpty)
        {
            if (string.IsNullOrEmpty(trieNode.RemainingKey))
            {
                trieNode.Entity = entity;
                return;
            }
            else
            {
                var newChild = new TrieNode(trieNode.RemainingKey[1..], trieNode.Entity);
                if (trieNode.Children.TryAdd(trieNode.RemainingKey[0], newChild))
                {
                    trieNode.RemainingKey = string.Empty;
                    trieNode.Entity = entity;
                }
                else
                {
                    var message = $"An entity with the same key has already been added. Key: {key.ToString()}";
                    throw new ArgumentException(message, nameof(key));
                }
            }
        }

        if (key == trieNode.RemainingKey)
        {
            trieNode.Entity = entity;
            return;
        }

        if (!trieNode.Children.TryGetValue(key[0], out var childTreeNode))
        {
            trieNode.Children.TryAdd(key[0], new TrieNode(key[1..].ToString(), entity));
        }
        else
        {
            if (!string.IsNullOrEmpty(childTreeNode.RemainingKey))
            {
                var newChild = new TrieNode(childTreeNode.RemainingKey[1..], childTreeNode.Entity);
                if (childTreeNode.Children.TryAdd(childTreeNode.RemainingKey[0], newChild))
                {
                    childTreeNode.RemainingKey = string.Empty;
                    childTreeNode.Entity = null;
                }
                else
                {
                    var message = $"An entity with the same key has already been added. Key: {key.ToString()}";
                    throw new ArgumentException(message, nameof(key));
                }
            }

            AddMathEntity(childTreeNode, key[1..], entity);
        }
    }

    private IMathEntity? FindMathEntity(TrieNode trieNode, ReadOnlySpan<char> expression)
    {
        if (!expression.IsEmpty && trieNode.Children.TryGetValue(expression[0], out var childTreeNode))
        {
            return FindMathEntity(childTreeNode, expression[1..]);
        }

        if (expression.StartsWith(trieNode.RemainingKey))
        {
            return trieNode.Entity;
        }

        return null;
    }

    #region private nested class TrieNode

    private class TrieNode(string remainingKey = "", IMathEntity? entity = null)
    {
        public ConcurrentDictionary<char, TrieNode> Children { get; } = new();

        public string RemainingKey { get; set; } = remainingKey;

        public IMathEntity? Entity { get; set; } = entity;
    }

    #endregion
}
