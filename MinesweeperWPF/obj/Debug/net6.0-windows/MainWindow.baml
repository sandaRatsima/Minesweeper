   M S B A M L   `   `   `  ����I  EMinesweeperWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:MinesweeperWPFMinesweeperWPF  5   	       MinesweeperWPF.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:MinesweeperWPF     5   	     � Title$  Minesweeper��5   	   $	��750��6   $	��750��6*   .��5	      �  � RowDefinitions 5
   
   �� 5      $��30 �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ LBLTitre��5      $��Mon Super Démineur��6"   $��0��6?   $��0,-3,0,3q�6L   $��Center=�6^   � 5   
   -   5         �Name$ GRDGame��5      $��Stretch=�6    $��Stretch=�6>   
�������$��1��6q    � ColumnDefinitions 5      �� 5      $
��384* �6#   �� 5      $
��217* �6#   5      �� 5   
   -   5        $ LBLTemps��5      $��Temps��6"   $��0��62   $��	456,0,4,0q�6?   $��Right=�6R   $
��Bold"�6u   $��16(�6�   $��Center=�6�   �� 5   
   -   5        $ LBLFlags��5      $��Flags : Nombre��6"   $��0��6;   $��
-1,0,478,0q�6H   $
��Bold"�6\   $��16(�6n   $��Center=�6|   �� 5   
   -   5         ��Name$ CBXLevel��5      $��20��6%   $��	0,0,118,0q�61   $��Center=�6D   $��Right=�6�   $��96��6�   
�������$��Center=�6�   ��5      �� 5       �� EndPoint$	 0,1&�6&   .��5      �  � Color$ 	#FFF0F0F0��6#   � 5      $ White��6#    � Offset$ 1Q�61   5      5      5      5       