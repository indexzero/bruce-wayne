module WithSyntax
{
  language WithLang 
  {
    syntax Main = with:WithStatement(Id) => with;
    
    syntax WithStatement(Context) 
      = "with" context:Context '{' statements:Statement* '}' => With { Context { context }, Statements { statements } };
  
    syntax Statement
      = id:Id op:AssignmentOperator val:Number ";" => Statement { Id { id }, Operator { op }, Value { val } };
      
    token Id = (Upper | Lower | '_') (Upper | Lower | Digit | '_')*;
    token AssignmentOperator  = '=' | '*=' | '/=' | '%=' | '+=' | '-=';	
    
    token Number
      =  Digit+ '.' Digit* Exponent?
      |   '.' Digit+ Exponent?
      |   Digit+ Exponent?;
    token Exponent 
      = ('e'|'E') ('+'|'-')? Digit+;
    
    token Upper = 'A'..'Z';
    token Lower = 'a'..'z';
    token Digit = '0'..'9';
    
    token Newline = '\r' | '\n' | '\r\n';
    token Whitespace = ' ' | '\t' | Newline;
    
    interleave Comment = '#' (any - Newline)* Newline;
    interleave Ignore = Whitespace+;
  }
}