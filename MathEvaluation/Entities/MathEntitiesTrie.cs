using System;
using System.Collections.Generic;

namespace MathEvaluation.Entities;

/// <summary>
///     A prefix tree, also known as a trie (pronounced "try"), is a type of search tree.
///     It is a special form of a digital tree that represents a dynamic set of strings,
///     usually words in a dictionary, in a way that allows for efficient retrieval.
/// </summary>
internal sealed class MathEntitiesTrie
{
    private readonly TrieNode _rootNode = new();

    public void AddMathEntity(IMathEntity entity)
        => AddMathEntity(_rootNode, entity.Key.AsSpan(), entity);

    public IMathEntity? FirstMathEntity(ReadOnlySpan<char> expression)
        => FirstMathEntity(_rootNode, expression);

    private static void AddMathEntity(TrieNode trieNode, ReadOnlySpan<char> key, IMathEntity entity)
    {
        while (!key.IsEmpty)
        {
            if (key == trieNode.RemainingKey)
            {
                trieNode.Entity = entity;
                return;
            }

            if (trieNode.Children.TryGetValue(key[0], out var childNode))
            {
                if (!string.IsNullOrEmpty(childNode.RemainingKey))
                    convertToInternalNode(childNode);

                trieNode = childNode;
                key = key[1..];
            }
            else
            {
                var newChild = new TrieNode(key[1..].ToString(), entity);
                trieNode.Children.Add(key[0], newChild);
                return;
            }
        }

        if (!string.IsNullOrEmpty(trieNode.RemainingKey))
            convertToInternalNode(trieNode);

        trieNode.Entity = entity;
        return;

        static void convertToInternalNode(TrieNode trieNode)
        {
            var newChild = new TrieNode(trieNode.RemainingKey[1..], trieNode.Entity);
            trieNode.Children.Add(trieNode.RemainingKey[0], newChild);
            trieNode.RemainingKey = string.Empty;
            trieNode.Entity = null;
        }
    }

    private static IMathEntity? FirstMathEntity(TrieNode trieNode, ReadOnlySpan<char> expression)
    {
        var i = 0;
        while (expression.Length > i && trieNode.Children.TryGetValue(expression[i], out var childNode))
        {
            trieNode = childNode;
            i++;
        }

        return expression[i..].StartsWith(trieNode.RemainingKey) ? trieNode.Entity : null;
    }

    #region private nested class TrieNode

    private class TrieNode(string remainingKey = "", IMathEntity? entity = null)
    {
        public Dictionary<char, TrieNode> Children { get; } = [];

        public string RemainingKey { get; set; } = remainingKey;

        public IMathEntity? Entity { get; set; } = entity;
    }

    #endregion
}