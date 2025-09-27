﻿using RobloxCS.AST.Expressions;

namespace RobloxCS.AST.Statements;

public sealed class CompoundAssignmentStatement : Statement {
    public required Var Right { get; set; }
    public required CompoundOp Operator { get; set; }
    public required Expression Left { get; set; }

    public override AstNode DeepClone() => throw new NotImplementedException();
    public override void Accept(IAstVisitor v) => v.VisitCompoundAssignmentStatement(this);
    public override T Accept<T>(IAstVisitor<T> v) => v.VisitCompoundAssignmentStatement(this);
}