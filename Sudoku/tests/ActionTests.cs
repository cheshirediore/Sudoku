using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sudoku;

namespace Sudoku.Tests;

public class ActionTests
{
    [Test]
    public void Action_Equality_ReturnsTrueIfEqual()
    {
        Action action1 = new(ActionType.SET, 16, 9);
        Action action2 = new(ActionType.SET, 16, 9);
        Assert.That(action1.Equals(action2), Is.True);
        Assert.That(action1==action2, Is.True); // this operator was not overwritten for some reason
    }

    [Test]
    public void Action_Equality_ReturnsFalseIfNotEqual()
    {
        Action action1 = new(ActionType.SET, 16, 9);
        Action action2 = new(ActionType.SET, 16, 1);
        Assert.That(action1.Equals(action2), Is.False);
        Assert.That(action1==action2, Is.False);
    }

    [Test]
    public void Action_ListContains_ReturnsTrueWhenPresent()
    {
        List<Action> actions = [];
        Action action1 = new(ActionType.SET, 16, 9);
        actions.Add(action1);

        Action action2 = new(ActionType.SET, 16, 9);
        Assert.That(actions.Contains(action2), Is.True);
    }

    [Test]
    public void Action_ListContains_ReturnsFalseWhenNotPresent()
    {
        List<Action> actions = [];
        Action action1 = new(ActionType.SET, 16, 9);
        actions.Add(action1);

        Action action2 = new(ActionType.SET, 16, 1);
        Assert.That(actions.Contains(action2), Is.False);

        Action action3 = new(ActionType.SET, 0, 9);
        Assert.That(actions.Contains(action3), Is.False);

        Action action4 = new(ActionType.REMOVE, 16, 9);
        Assert.That(actions.Contains(action4), Is.False);
    }
}