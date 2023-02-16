### CallFrame

CallFrame is an eviroment for running code. It contains:
- Stack - a place to store data required for running code
- Instruction pointer - a pointer to current running code position 
- Code - a code instructions to run
- Additional information: this class pointer, name or number of this class running method to get a code from it
- pointer to a previous CallFrame - a place to return a result of running code when it coplete


Фрейм стека (Stack Frame) - это структура данных, используемая для поддержки вызова метода и выполнения 
метода виртуальной машиной, и он является элементом стека стека виртуальной машины (стека виртуальной машины) 
в области данных виртуальной машины во время выполнения. Кадр стека сохраненТаблица локальных переменных 
метода, стек операндов, динамическая ссылка и адрес возврата методаИ другая информация. 
Каждый метод от начала вызова до завершения выполнения соответствует 
кадру стека из стека в стек в стеке виртуальной машины.

Каждый кадр стека включает в себя таблицу локальных переменных, стек операндов, динамическое соединение, 
адрес возврата метода и некоторую дополнительную дополнительную информацию.

### Thread

Thread contains CallStack

Thread in PhantomOs like a physical processor or core in physical computer system. It perform execution of class method
command and held a desired enviorment for it. Можно сказать что Thread это разделение одного физического процессора на
множество виртуальных.

Как правило ОС не выделяет отдельный процессор под каждый процесс, а значит применяется 
процедура time slicing (нарезка времени) – процессор постоянно переключается между потоками выполнения. 
Переключение происходит сотни раз в секунду (тактовая частота), и со стороны кажется, 
что все потоки работают одновременно – но это не так.

Пото́к выполне́ния (тред; от англ. thread — нить) — наименьшая единица обработки, 
исполнение которой может быть назначено ядром операционной системы. 

Несколько ядер могут решать список задач параллельно.
Тогда одновременно будет выполняться более одного потока.

В каком порядке запускать новые потоки решает Планировщик потоков – часть JVM, которая решает какой поток 
должен выполнится в каждый конкретный момент времени и какой поток нужно приостановить.


### Referenced

Description of JVM and Stack vs Register VM comparision https://russianblogs.com/article/6121494288/

https://docs.oracle.com/javase/specs/jvms/se7/html/jvms-2.html
A frame is destroyed when its method invocation completes, whether that completion is 
normal or abrupt (it throws an uncaught exception). 
Frames are allocated from the Java Virtual Machine stack (§2.5.2) of the thread creating the frame
