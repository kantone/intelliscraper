   M S B A M L   `   `   `  ����J  FIscraperBuilder, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:IscraperBuilder.Controls.CommonIscraperBuilder.Controls.Common  5      6    0IscraperBuilder.Controls.Rules.Rule.ruleDownload   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <my-clr-namespace:IscraperBuilder.Controls.Common     5        N� Title$  ruleDownload��6   .��5
      � .+�5   
   �� -   5        $��tabControl1��5   F   $��Stretch=�6   $��0,40,0,0q�64   $��Stretch=�6Y   .@�5      �� -   5        $��tabItem1��5   (   $��Download��6   .��5      � .+�5      �� -   5        $��chkAutoRename��5   o   $��Auto rename��6   $��16��65   $
��Left=�6A   $��	121,6,0,0q�6\    � ToolTip$* $Rename download image with timestamp��6�   $	��Top=�6�   $	��120��6�   �� 5      -   5        $��label2��5   s   $��Input attribute key��6   $��28��6:   $
��Left=�6F   $��7,26,0,0q�6a   $	��Top=�6�   �� 5      -   5        $��txtAttributeKey��5   �   $��23��6   $
��Left=�6*   $# Format : action.ruleAttribute��6E   $��
121,28,0,0q�6m   $	��Top=�6�   $	��195��6�   �� 5      -   5        $��label3��5   n   $��File extension��6   $��28��65   $
��Left=�6A   $��7,89,0,0q�6\   $	��Top=�6|   �� 5      -   5        $��
txtFileExt��5   Y   $��23��6   $
��Left=�6*   $��
121,91,0,0q�6E   $2 ,Format supported : jpg,jpeg,png,tiff,ico,bmp��6k   $	��Top=�6�   $	��110��6�   �� 5      -   5        $��chkSetFileExt��5   w   $��Set file extension��6   $��16��6<   $
��Left=�6H   $��
121,69,0,0q�6c   $* $Rename download image with timestamp��6�   $	��Top=�6�   $	��120��6�   �� 5      -	   5        $��label4��5   r   $��Custom user agent��6   $��28��68   $
��Left=�6D   $��	7,138,0,0q�6_   $	��Top=�6�   �� 5      -
   5        $��txtCustomUserAgent��5   Z   $��23��6   $
��Left=�6*   $��121,138,0,0q�6E   $2 ,Format supported : .jpg,.png,.tiff,.ico,.bmp��6t   $	��Top=�6�   $	��407��6�   9� 5      $��Http headers��6   $��
7,173,77,8q�65   $	��307��6I   $	��656��6V   .��5      4   .IscraperBuilder.Controls.Common.HttpHeaderInfo  -   5          Name$ httpHeaderInfo1��5   ,   5      5      5      �� 5      -   5        $��tabItem2��5   :   $ ��Directory and save options��6   .��5      � .+�5      �� -   5        $��label5��5   l   $��Start folder��6   $��28��63   $
��Left=�6?   $��6,17,0,0q�6Z   $	��Top=�6z   �� 5      -   5        $��txtStartFolder��5   Y   $��23��6   $
��Left=�6*   $��
156,19,0,0q�6E   $( "Where you want to save your files?��6o   $	��Top=�6�   $	��310��6�   �� 5      -   5        $��button1��5   q   $��Select folder��6   $��23��65   $��Right=�6A   $��
0,19,172,0q�6]   $	��Top=�6�   $��96��6�   6�   �� 5       -   5        $��chkGenerateDir��5    �   $%��Generate sub folders structure?��6   $��16��6I   $
��Left=�6U   $��
157,91,0,0q�6p   $	��Top=�6�   6�   �� 5!      -   5        $��cmbFolderGenType��5!   [   $��23��6   $
��Left=�6+   $��157,144,0,0q�6F   $	��Top=�6s   $	��305��6�   6�   �� 5"      -   5        $��label6��5"   p   $��Generation type��6   $��28��66   $
��Left=�6B   $��	7,139,0,0q�6]   $	��Top=�6~   �� 5#      -   5        $��label7��5#   w   $��Sub folder custom name��6   $��28��6=   $
��Left=�6I   $��	7,113,0,0q�6d   $	��Top=�6�   �� 5$      -   5        $��txtSubFolderCustomName��5$   Z   $��23��6   $
��Left=�6*   $��157,113,0,0q�6E   $( "Where you want to save your files?��6x   $	��Top=�6�   $	��183��6�   5%      5&      �� 5'      -   5        $��tabItem3��5'   3   $��MultiThread options��6   .��5(      � .+�5)      �� -   5        $��chkMTEnable��5)   ~   $��Enable parallel download?��6   $��16��6C   $
��Left=�6O   $��
139,16,0,0q�6j   $	��Top=�6�   �� 5*      -   5        $��chkMTSetMaxThread��5*   �   $)��#Set max numbers of parellel process��6   $��16��6M   $
��Left=�6Y   $��
139,38,0,0q�6t   $	��Top=�6�   �� 5+      -   5        $��label1��5+   t   $��Max parallel process��6   $��28��6;   $
��Left=�6G   $��7,62,0,0q�6b   $	��Top=�6�   �� 5,      -   5        $��txtMTMaxThread��5,   Y   $��23��6   $
��Left=�6*   $��
139,62,0,0q�6E   $	��Top=�6o   $��62��6�   5-      5.      �� 5/      -   5        $��tabItem4��5/   -   $��Image Convert��6   .��50      � .+�51      2   ,IscraperBuilder.Controls.Common.ImageConvert  -   5          Name$ imageConvert1��51   D   $��Stretch=�6&   $��Stretch=�6[   52      53      54      �� 55   
   -   5        $��button2��55   Z   $
��Save��6   $��23��6    $
��Left=�6,   $��	588,8,0,0q�6G   $	��Top=�6i   $��75��6�   6�   �� 56   
   -   5        $��txtID��56   N   $��23��6   $
��Left=�6    $��	12,12,0,0q�6;   $��id��6[   $
��Bold"�6e    J� Opacity$	 0.7Q�6w   $	��Top=�6�   57      58      