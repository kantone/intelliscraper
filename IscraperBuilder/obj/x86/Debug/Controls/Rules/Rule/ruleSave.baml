   M S B A M L   `   `   `  ����  IscraperBuilder2    ,IscraperBuilder.Controls.Rules.Rule.ruleSave   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  ruleSave��5	      .��5      � .+�5   
   �� -   5        $��tabControl1��5   5   $��Stretch=�6   $��0,40,0,0q�6H   $��Stretch=�6Z   .@�5      �� -   5        $��tabItem1��5   )   $��	Save info��6   .��5      � .+�5      �� -   5        $��label1��5   i   $��Store id��6   $��28��6/   $
��Left=�6;   $��	15,15,0,0q�6V   $	��Top=�6w   �� 5      -   5        $��
cmbStoreId��5   Y   $��23��6   $
��Left=�6+   $��	92,15,0,0q�6F   $	��Top=�6k   $	��219��6�   6�   �� 5      -   5        $��label2��5   k   $��
Table name��6   $��28��61   $
��Left=�6=   $��	15,49,0,0q�6X   $	��Top=�6y   �� 5      -   5        $��txtTableName��5   X   $��23��6   $
��Left=�6*   $��	92,49,0,0q�6E   $	��Top=�6l   $	��219��6�   5      5      �� 5      -   5        $��tabItem2��5   '   $��Mapping��6   .��5      �  � ColumnDefinitions 5      �� 5      $��25 �6+   �� 5      $	��615 �6+   .+�5      &   System.Windows.Controls.DataGrid  -   5        $��	dataGrid1��5   y      AutoGenerateColumns .  $	��343��6;   $��Stretch=�6H   $��	0,139,0,0q�6f   $	��Top=�6�   $��2��6�   ��6�   ��    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Action Id��61   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6D   ��  �� Path$ actionId�  5      $ Column Name��61    6F   �� $ 
mapColName�  5      $ Attribute id��61    6G   �� $ attributeId�  5       $ Is xpath collection��61    6N   �� $ inputIsXpathCollection�  5!      $ xpath collection position��61    6U   �� $ xpathCollectionPosition�5#      �� 5$      -	   5        $��label7��5$   z   $!��Map column name by store id��6   $��28��6B   $
��Left=�6N   $��6,6,0,0q�6i   $	��Top=�6�   $��2��6�   $	��176��6�   �� 5%      -
   5        $��cmbMapColName��5%   �   $��23��6    � ToolTip$. (Selection avaiable only for excel or csv��6+   $
��Left=�6^   $��	163,6,0,0q�6y   $	��Top=�6�   $	��162��6�   $��1��6�   6�   �� 5&      -   5        $��label8��5&   �   $��Map attribute id��6   $��2��67   $��28��6K   $
��Left=�6W   $��6,60,0,0q�6r   $	��Top=�6�   $	��125��6�   �� 5'      -   5        $��chkInputIsCollection��5'   �   $��Input is collection?��6   $M Gif input is collection of object ( loop_link,or xpathCollection,etc...)��6>   $��16��6�   $
��Left=�6�   $��
163,89,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5(      -   5        $��label9��5(   �   $��Collection value position��6   $��2��6@   $��28��6T   $
��Left=�6`   $��	5,105,0,0q�6{   $	��Top=�6�   $	��149��6�   �� 5)      -   5        $��txtXpathCollPosition��5)   �   $��1��6   $��23��6.   $
��Left=�6:   $��0��6U   $- 'If is collection find value by position��6^   $��163,106,0,0q�6�   $	��Top=�6�   $��73��6�   �� 5*      -   5        $��button2��5*   w   $	��Add��6   $��1��6+   $��23��6;   $
��Left=�6G   $��329,105,0,0q�6b   $	��Top=�6�   $��75��6�   6�   �� 5+      -   5        $��button2a��5+   �   $��Remove Selected��6   $��1��67   $��23��6G   $
��Left=�6S   $��410,106,0,0q�6n   $	��Top=�6�   $	��106��6�   6�   �� 5,      -   5        $��txtMapColName��5,   i   $��1��6   $��23��6.   $��Right=�6:   $��	0,8,107,0q�6V   $	��Top=�6~   $	��162��6�   �� 5-      -   5        $��txtAttributeId��5-   Y   $��23��6   $
��Left=�6*   $��
346,62,0,0q�6E   $	��Top=�6o   $	��162��6�   $��1��6�   �� 5.      -   5        $��label6��5.   l   $��	Action id��6   $��28��60   $��Right=�6<   $��
0,35,577,0q�6X   $	��Top=�6z   $��2��6�   �� 5/      -   5        $��cmbMapActionId��5/   j   $��1��6   $��23��6/   $
��Left=�6;   $��
163,35,0,0q�6V   $	��Top=�6�   $	��162��6�   6�   �� 50      -   5        $��cmbAttributes��50   j   $��1��6   $��23��6/   $
��Left=�6;   $��
163,62,0,0q�6V   $	��Top=�6   $	��162��6�   6�   51      52      �� 53      -   5        $��tabItem3��53   0   $��On exist Actions��6   .��54      � .+�55      �� -   5        $��chkCheckExist��55   �   $D��>Check if this item exist in storage(file or database, or .. )?��6   $��16��6h   $
��Left=�6t   $��
146,18,0,0q�6�   $	��Top=�6�   �� 56      -   5        $��label3��56   w   $��Check exist attributes��6   $��28��6>   $
��Left=�6J   $��6,38,0,0q�6e   $	��Top=�6�   �� 57      -   5        $��txtCheckExistAttrs��57   Y   $��23��6   $
��Left=�6*   $��
146,40,0,0q�6E   $	��Top=�6s   $	��333��6�   $G Alist of attributes to match exist. Separated by custom separator.��6�   �� 58      -   5        $��label4��58   i   $��	Separator��6   $��28��60   $
��Left=�6<   $��6,72,0,0q�6W   $	��Top=�6w   �� 59      -   5        $��txtCheckExistSeparator��59   Y   $��23��6   $
��Left=�6*   $��
146,69,0,0q�6E   $G Alist of attributes to match exist. Separated by custom separator.��6w   $	��Top=�6�   $��51��6�   �� 5:      -   5        $��label5��5:   p   $��If exist action��6   $��28��66   $
��Left=�6B   $��	5,106,0,0q�6]   $	��Top=�6~   �� 5;      -   5        $��cmbExistAction��5;   [   $��23��6   $
��Left=�6+   $��146,106,0,0q�6F   $	��Top=�6q   $	��178��6�   5<      5=      5>      �� 5?   
   -   5        $��txtId��5?   `   $
��Bold"�6   $��23��6&   $
��Left=�62   $��	10,10,0,0q�6M    J� Opacity$	 0.7Q�6m   $��ID��6{   $	��Top=�6�   �� 5@   
   -   5        $��button1��5@   Z   $
��Save��6   $��24��6    $
��Left=�6,   $��	501,6,0,0q�6G   $	��Top=�6i   $��75��6�   6�   5A      5B      