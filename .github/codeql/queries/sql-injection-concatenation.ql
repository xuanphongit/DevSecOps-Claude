/**
 * @name SQL Injection via String Concatenation
 * @description Detects SQL injection vulnerabilities where user input is concatenated into SQL queries using string concatenation (+)
 * @id cs/custom/sql-injection-concatenation
 * @kind path-problem
 * @problem.severity error
 * @precision high
 * @tags security
 * @tags external/cwe/cwe-089
 */

import csharp
import semmle.code.csharp.security.SqlInjection
import DataFlow::PathGraph

/**
 * Detects SQL injection where user input flows through string concatenation into SQL queries
 */
class SqlInjectionViaConcatenation extends SqlInjectionSink {
  SqlInjectionViaConcatenation() {
    exists(StringConcatenationExpr concat |
      // The SQL command is built using string concatenation
      this.getQuery().getAChild*() = concat and
      // One of the operands is user-controlled
      exists(DataFlow::Node source |
        source.asExpr() = concat.getAnOperand() and
        source instanceof RemoteFlowSource
      )
    )
  }
}

from SqlInjectionViaConcatenation sink, DataFlow::PathNode source, DataFlow::PathNode sinkNode
where sinkNode.asNode() = sink and
  source.asNode() instanceof RemoteFlowSource and
  sinkNode.asNode().getQuery().getAChild*() = source.asNode().asExpr().getAChild*()
select sinkNode, source, sinkNode,
  "SQL injection vulnerability: user input '$@' is concatenated into SQL query",
  source.getNode(), sinkNode.getNode()
