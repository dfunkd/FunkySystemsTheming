   M S B A M L   `   `   `  ����J  FRecipesWithFunk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:RecipesWithFunk.WindowsRecipesWithFunk.Windows  5   	   O,clr-namespace:RecipesWithFunk.CustomControlsRecipesWithFunk.CustomControls  5   	   G(clr-namespace:RecipesWithFunk.ViewModelsRecipesWithFunk.ViewModels  5   	   3    -RecipesWithFunk.Windows.RecipesWithFunkWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   7local%clr-namespace:RecipesWithFunk.Windows     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;cc,clr-namespace:RecipesWithFunk.CustomControls     5   	   7vm(clr-namespace:RecipesWithFunk.ViewModels     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $	��850��5	   	   $
��1200��5
   	   
������   � 
ResizeMode$  NoResize=�5   	    � Title$ RecipesWithFunkWindow��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � WindowStyle$
 None=�5   	    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5        3   -RecipesWithFunk.Windows.RecipesWithFunkWindow   CloseCommand   Command# Z 5      6{    � DataContext 5      9   3RecipesWithFunk.ViewModels.RecipesWithFunkViewModel  5   
   .��5      �  
  Darker#���   6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��7* �6   �� 5      $��* �6   	 � RowDefinitions	 5!   
   �� 5"      $
��Auto �6   �� 5#      $
��Auto �6   �� 5$      $��* �6   �� 5%      $
��Auto �6   .+�5(   
   �� -   5        $��0��5(      $��3��6!   $��0��65     
HeaderFade#���  6B   $��0q�6l   
 �� CornerRadius$
 0, 0, 10, 10��6�    �� Padding$ 0, 0, 0, 15q�6�   $��
0, 0, 0, 5q�5)      ._�5*      �  5+      �� 5,      $��55 �6'   �� 5-      $��5* �6'   �� 5.      $��55 �6'   .+�51      �� $��1��6   $��Center=�6-    �� Orientation$ 
Horizontal=�6J   .+�52      ��   DarkerHeaderFade#���  6    $
��Bold"�6P   $��24(�6b   $
��Left=�6p   $��10, 8, 5, 0q�6�   $��Recipes With��53       $��Center=�64   6   0RecipesWithFunk.CustomControls.OutlinedTextBlock  54         
FontFamily$ Comic Sans MS)�6+      FontSize$ 48(�6F      
FontWeight$
 Bold"�6T     RuanRedFade   Fill# �  6f   $��Right=�6�   $��15, 0q�55         Text$
 Funk��6.   $��Center=�6:   56      �� 58      $��2��6   $.��(local:RecipesWithFunkWindow.CloseCommand��6)   $��25��6\   $
��5, 0q�6h   $��Center=�6v   $��25��6�   ��59      �� 5:      $��X��6$   5<      5=      5>      �� 5@   
   $��1��6   $��1��6!     Darkest#���  6.   $��1q�6V   $
 10��6j     Lighter#���  6|   $��Center=�5A      $��0, -25, 0, 10q�6.   ._�5B      � -   5         �Name$ wpMenu��5B       � Orientation$ 
Horizontal=�6(   .+�5C      d� -   5         d�Name$ tbHome��5C      $
��Home��6D    d� 	IsChecked$
 TrueY�6S   $��
5, 5, 2, 5q�6d   d� 5D      -   5        $ tbView��5D      $
��View��6D   $
��2, 5q�6S   d� 5E      -   5        $ tbAdd��5E      $	��Add��6C   $��
2, 5, 5, 5q�6Q   5F      5G      ]� 5I   
   -   5         ]�Name$ 	dpContent��5I      $��0��6'   $��3��67   $��2��6K   $��10, 0, 10, 10q�6X     5K   
   $��1��6   $��3��6/   $ Comic Sans MS)�6<   $ 18(�6W   $
 Bold"�6e   # �  6w   $��Right=�5L      $��0, -5, 0, 0q�6;   $��Center=�6k    6P   �� *	CopyRight+6k   5M      5N      