   M S B A M L   `   `   `  ����A  =Tetera, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:TeteraTetera  5   	   R NSharpGL.WPF, Version=3.1.1.0, Culture=neutral, PublicKeyToken=27fc851303210b27>.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPFSharpGL.WPF 5
   	       Tetera.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Tetera     5   	   BsharpGL.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPF     5
   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5      � .+�5   
     SharpGL.WPF.OpenGLControl  -   5          Name$ openGLControl��5      $��	247,0,0,0q�5      $
��Left=�5       J� RenderTransformOrigin$ 0.5,0.5&�63   $	��553��6S   z�5      V� 5      .~�5      ��  �� ScaleX$ -1Q�6%   �� 5      �� 5      T� 5      5      5      �� 5   
    �� Orientation$ Vertical=�6   $��10q�6,   .+�5      �� -   5         ��Name$ 
chkAmbient��5      $��Modelo de luz Ambiental��6+    d� 	IsChecked$
 TrueY�6M   �� 5      -   5        $ chkGlobalAmbient��5      $$��Modelo de Luz Ambiental Global��61   $
 TrueY�6Z   �� 5       -   5        $ chkLightPosition��5       $��Posición de la Luz��61   $
 TrueY�6N   �� 5!      -   5        $ chkAmbientLight��5!      $��Luz Ambiental��60   $
 TrueY�6H   $��0,10,0,0q�6Y   �� 5"      -   5        $ chkDiffuseLight��5"      $��
Luz Difusa��60   $
 TrueY�6E   �� 5#      -   5        $ chkSpecularLight��5#      $��Luz Especular��61   $
 TrueY�6I   5$      5%      5&      