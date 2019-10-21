declare

long_to_char varchar2(1000);

type ti_row is record(
table_name varchar2(50),
column_name varchar2(50),
column_type varchar2(50),
nullable char(1),
comments varchar2(1000),
PK number(2,0),
cons varchar2(2000),
fk_table varchar2(250)
);
type tcols IS TABLE OF ti_row index by binary_integer;
type ttables IS TABLE OF tcols index by binary_integer;

tab_defs ttables;
tab_ind binary_integer;
col_ind binary_integer;

str varchar2(1000);
str_part varchar2(256);

i binary_integer;

begin
for i in (
select rank() over ( order by c.table_name) tab_rn
,c.table_name
   	 ,c.column_id
	 ,c.column_name
	 ,case when c.data_type like '%CHAR%' then data_type||'('||c.data_length||')'
	  when c.data_type in ('DATE','TIMESTAMP','TIME') then c.data_type
	  when c.data_precision is null then c.data_type
    else c.data_type||'('||c.data_precision||','||nvl(c.data_scale,0)||')' end col_type
	 ,nullable
	 ,colc.comments
   ,cn.constraint_type
   ,decode(cn.constraint_type,'P',conc.position) PK
   ,cn.search_condition
   ,decode(cn.constraint_type,'U','unique('||cn.index_name||')') Unq
   ,rc.CONSTRAINT_NAME r_cons
   ,rc.table_name r_table
   ,rc.owner r_owner
from all_tables t
    ,all_tab_columns c 
    ,all_col_comments colc 
    ,all_cons_columns conc  
    ,all_constraints cn  
    ,all_constraints rc  
where t.owner = (select SYS_CONTEXT( 'USERENV', 'CURRENT_USER' ) from dual) 
and t.owner = c.owner
and t.table_name = c.table_name
and c.owner = colc.owner and 	c.table_name = colc.table_name and   c.column_name = colc.column_name
and c.OWNER = conc.OWNER(+) and   c.TABLE_NAME = conc.TABLE_NAME(+) and   c.COLUMN_NAME = conc.COLUMN_NAME(+)
and cn.owner(+) = conc.owner and   conc.TABLE_NAME = cn.TABLE_NAME(+) and   cn.CONSTRAINT_NAME(+) = conc.CONSTRAINT_NAME 
and cn.r_owner = rc.owner(+) and cn.r_CONSTRAINT_NAME = rc.CONSTRAINT_NAME(+)
order by c.table_name
        ,c.column_id
        ,rc.CONSTRAINT_NAME desc) loop
        
        
       select replace(i.search_condition,chr(10),'') into long_to_char from dual;
         
        if tab_defs.EXISTS(i.tab_rn) and  tab_defs(i.tab_rn).EXISTS(i.column_id) then 
           if long_to_char is not null then 
            if  tab_defs(i.tab_rn)(i.column_id).cons is null then 
              tab_defs(i.tab_rn)(i.column_id).cons := long_to_char;
            else
              if tab_defs(i.tab_rn)(i.column_id).cons not like '%'||long_to_char||'%' then
                tab_defs(i.tab_rn)(i.column_id).cons := tab_defs(i.tab_rn)(i.column_id).cons || ';' ||long_to_char;
              end if;
            end if;  
          end if;  
          if i.unq is not null then 
            if  tab_defs(i.tab_rn)(i.column_id).cons is null then 
              tab_defs(i.tab_rn)(i.column_id).cons := i.unq;
            else
              if tab_defs(i.tab_rn)(i.column_id).cons not like '%'||i.unq||'%' then
                tab_defs(i.tab_rn)(i.column_id).cons := tab_defs(i.tab_rn)(i.column_id).cons || ';' ||i.unq;
              end if;
            end if;  
          end if;  
          if tab_defs(i.tab_rn)(i.column_id).PK is null then 
             tab_defs(i.tab_rn)(i.column_id).PK := i.PK;
          end if;  
          if i.r_table is not null then
            if tab_defs(i.tab_rn)(i.column_id).fk_table is null then
               tab_defs(i.tab_rn)(i.column_id).fk_table := i.r_owner||'.'||i.r_table||'('||i.r_cons||')';
            elsif tab_defs(i.tab_rn)(i.column_id).fk_table not like '%'||i.r_owner||'.'||i.r_table||'('||i.r_cons||')'||'%' then
               tab_defs(i.tab_rn)(i.column_id).fk_table := tab_defs(i.tab_rn)(i.column_id).fk_table || ';' ||i.r_owner||'.'||i.r_table||'('||i.r_cons||')';
            end if;   
          end if;
        else 

		  tab_defs(i.tab_rn)(i.column_id).table_name := i.table_name;
            tab_defs(i.tab_rn)(i.column_id).column_name := i.column_name;
            tab_defs(i.tab_rn)(i.column_id).column_type := i.col_type;
            tab_defs(i.tab_rn)(i.column_id).nullable    := i.nullable;
            tab_defs(i.tab_rn)(i.column_id).comments    := replace (i.comments,chr(10),'');
            tab_defs(i.tab_rn)(i.column_id).PK          := i.PK;
            tab_defs(i.tab_rn)(i.column_id).cons        := long_to_char;
            if i.r_table is not null then
              tab_defs(i.tab_rn)(i.column_id).fk_table    := i.r_owner||'.'||i.r_table||'('||i.r_cons||')';
            end if;
        end if;        
        end loop;
    
        tab_ind := tab_defs.FIRST;
        
        WHILE (tab_ind IS NOT NULL)
         LOOP
            col_ind := tab_defs(tab_ind).FIRST;
            
            WHILE (col_ind IS NOT NULL)
             LOOP
	             str := tab_defs(tab_ind)(col_ind).column_name || ',' || tab_defs(tab_ind)(col_ind).column_type || ',' || tab_defs(tab_ind)(col_ind).NULLABLE || ',' || tab_defs(tab_ind)(col_ind).PK || ',' || tab_defs(tab_ind)(col_ind).fk_table || ',' || tab_defs(tab_ind)(col_ind).cons;
	             insert into TEMP_TO_FORMAT (recor) values (tab_defs(tab_ind)(col_ind).table_name || ',' || str);
	             col_ind := tab_defs(tab_ind).NEXT(col_ind); 
             END LOOP;
            tab_ind := tab_defs.NEXT(tab_ind); 
         END LOOP;

end;