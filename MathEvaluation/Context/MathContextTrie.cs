using System;
using System.Collections.Generic;

namespace MathEvaluation.Context;

/// <summary>
/// A prefix tree, also known as a trie (pronounced "try"), is a type of search tree. 
/// It is a special form of a digital tree that represents a dynamic set of strings, 
/// usually words in a dictionary, in a way that allows for efficient retrieval.
/// </summary>
internal sealed class MathContextTrie
{
    private TrieNode _rootNode = new();

    public void AddMathOperand(IMathOperand operand)
    {
        AddMathOperand(_rootNode, operand.Name.AsSpan(), operand);
    }

    public IMathOperand? FindMathOperand(ReadOnlySpan<char> expression)
    {
        return FindMathOperand(_rootNode, expression);
    }

    private void AddMathOperand(TrieNode trieNode, ReadOnlySpan<char> name, IMathOperand operand)
    {
        if (name.IsEmpty)
        {
            if (string.IsNullOrEmpty(trieNode.RemainingKey))
            {
                trieNode.Operand = operand;
                return;
            }
            else
            {
                trieNode.Children.Add(trieNode.RemainingKey[0], new TrieNode(trieNode.RemainingKey[1..], trieNode.Operand));
                trieNode.RemainingKey = string.Empty;
                trieNode.Operand = operand;
                return;
            }
        }

        if (name == trieNode.RemainingKey)
        {
            trieNode.Operand = operand;
            return;
        }

        if (!trieNode.Children.TryGetValue(name[0], out var childTreeNode))
        {
            trieNode.Children.Add(name[0], new TrieNode(name[1..].ToString(), operand));
        }
        else
        {
            if (!string.IsNullOrEmpty(childTreeNode.RemainingKey))
            {
                var newChild = new TrieNode(childTreeNode.RemainingKey[1..], childTreeNode.Operand);
                childTreeNode.Children.Add(childTreeNode.RemainingKey[0], newChild);
                childTreeNode.RemainingKey = string.Empty;
                childTreeNode.Operand = null;

            }

            AddMathOperand(childTreeNode, name[1..], operand);
        }
    }

    private IMathOperand? FindMathOperand(TrieNode trieNode, ReadOnlySpan<char> expression)
    {
        if (!expression.IsEmpty && trieNode.Children.TryGetValue(expression[0], out var childTreeNode))
        {
            return FindMathOperand(childTreeNode, expression[1..]);
        }

        if (expression.StartsWith(trieNode.RemainingKey))
        {
            return trieNode.Operand;
        }

        return null;
    }

    #region private nested class TrieNode

    private class TrieNode(string remainingKey = "", IMathOperand? operand = null)
    {
        public Dictionary<char, TrieNode> Children { get; } = new();

        public string RemainingKey { get; internal set; } = remainingKey;

        public IMathOperand? Operand { get; internal set; } = operand;
    }

    #endregion
}
