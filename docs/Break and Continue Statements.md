# Break and Continue Statements

Consider the following code:

```c
while (1)
{
	int condition = GetSomeCondition();
	if (condition) { break; }
	
	SignalCondition(condition);
}
```

We want the break statement to:
	- Reset the stack to where it was before the while loop, so that `condition` is removed
	- Jump to after the while loop

Consider this code:

```c
while (1)
{
	int condition = GetSomeCondition();
	if (!condition) { continue; }
	
	SignalCondition(condition);
}
```

We want the continue statement to:
	- Reset the stack to where it was before the while loop, so that `condition` can be reallocated without ever increasing the stack
	- Jump to the start of the while loop
	
Consider this code:

```c
switch (i)
{
	case 0:
	{
		Signal0();
	}
	case 1:
	{
		Signal1();
		break;
	}
	default:
	{
		SignalSomethingElse(i);
		break;
	}
}
```

We want each break to:
	- Reset the stack to where it was before the switch statement
	- Jump to after the entire switch block
	
We want `case 0` to flow into `case 1`.

## Solution

- Let while loops, do-while loops, and switch statements be _breakable statements_.
- When `Generate` is called on a breakable statement, a _break context_ is pushed on a stack in `EmitContext`, consisting of the current stack size and whether the breakable statement is a loop and therefore supports `continue`. At the end of `Generate`, the _break context_ on the top of the stack is popped.
- When `Generate` is called on a break or continue statement, it emits an instruction that resets the stack to the size stored in the topmost break context. It returns an unconnected jump of new type `ToBreakTarget` or `ToContinueTarget`.
- Inside `Generate` on a loop, continue-target unconnected jumps are connected to the start of the loop, as it is currently.
- Blocks, when they come across a break-target unconnected jump, will connect that jump to the statement FOLLOWING the statement with the unconnected jump IFF the statement with the unconnected jump is a breakable statement.
- If the statement with the unconnected jump is the last statement in the block, it is passed up again to the next higher level.
- Functions always connect any statements with unconnected jumps to the next statement without caring where the jump came from.

## Working with Existing Code

- Conditional statements have the following unconnected jumps:
	- From true flow to after target
	- From false flow to after target, if false flow present, OR
	- From comparison flow to after target, if no false flow