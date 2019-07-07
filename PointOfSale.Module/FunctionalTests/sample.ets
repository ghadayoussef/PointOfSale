;#DropDB PointOfSaleEasyTest

#Application PointOfSaleWin
#Application PointOfSaleWeb
#IfDef PointOfSaleWin

*Action About...
 
*Action OK

#EndIf

#IfDef PointOfSaleWeb
!Action Navigation(My Details)
#EndIf
