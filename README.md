Программа опрашивает контроллер по таймеру и пересылает 
значение переменной Eth_var, которая является структурой
из 5 полей произвольно именованных типа LREAL.
Пересылает значения в канал ZMQ tcp://127.0.0.1:5550 как 
набор байтов не преобразовывая.
Ответный скрипт является python-addon для Blender. Он принимает 
значения переменной и устанавливает параметры риг-модели в соответвии
с ними, таким образом 3D модель отображается в соответсвии со 
значаением осей манипулятора.