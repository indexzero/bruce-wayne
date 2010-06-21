grammar withLanguage;

tokens 
{
  WITH;
  STATEMENTS;
  STATEMENT;
}

withStatement 
    : 'with' ID '{' (statements += statement($ID.text))* '}'
      -> ^(WITH ^(STATEMENTS $statements*)
    ;
    
statement[String with]
    : id = ID op = ASSIGNMENT_OPERATOR val = NUMBER ';'
    ;
	
ID  :	('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*;
ASSIGNMENT_OPERATOR      : ':=' | '*=' | '/=' | '%=' | '+=' | '-=';
NUMBER :   ('0'..'9')+ '.' ('0'..'9')* EXPONENT?
       |   '.' ('0'..'9')+ EXPONENT?
       |   ('0'..'9')+ EXPONENT?;
fragment EXPONENT : ('e'|'E') ('+'|'-')? ('0'..'9')+ ;
