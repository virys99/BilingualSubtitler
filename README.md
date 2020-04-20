<img class=" size-medium wp-image-1366 alignleft" src="https://0xothik.files.wordpress.com/2016/02/logo.jpg?w=300" alt="logo" width="300" height="262" />

<p style="text-align: center;"><strong><span style="color: #999999;">Bilingual</span> <span style="color: #ffd700;">Subtitler</span></strong></p>
Программа для повышения удобства и результативности просмотра кино и сериалов на английском языке с английскими субтитрами при помощи динамически подключаемых (или постоянно постоянно подключенных к английским) русских субтитров.

Как именно? Вам нужно загрузить в программу английские и интересующие вас русские субтитры (или создать оные в приложении через Яндекс.Переводчик).
Затем:
— Смотрите видео с английскими субтитрами. При появлении субтитра, вызывающего затруднения, нажмите горячую клавишу, видео остановится, к английским субтитрам на экране добавятся русские. После прочтения вариантов перевода, вновь нажмите на горячую клавишу, русские субтитры уберутся, пауза снимется, просмотр продолжится с английскими субтитрами
— Либо смотрите видео с двуязычными субтитрами — с настройкой внешнего вида и местоположения как английских, так и русских.

Зачем? Лично мне так интереснее смотреть кино :) Просто его смотреть — не так часто попадаются картины, после которых не остается чувства потраченного времени, но вот примешивая к этому практику языка — получается достаточно занятный способ времяпрепровождения :) Пересматривать так фильмы — вообще отлично. Опять же, можно посмотреть, как справились разные переводчики.
Но вы можете использовать программу для того, для чего вам заблагорассудится :)

Я видел в интернете некоторое количество сервисов, предлагающих просмотр на английском с английскими субтитрами, однако у них были либо одна из, либо все проблемы:
1. Предлагалось каждый раз останавливать видео и выбирать непонятное слово из субтитров, чтобы узнать его перевод.
2. Неизвестно какое качество перевода.
3. «Ассортимент» видео — ограничен тем, что загружено на сервис.
Bilingual Subtitler же работает с любыми видео, которые у вас есть, любыми видеоплеерами (поддерживающими горячие клавиши паузы/смены субтитров, подробнее смотрите в разделе «Требования»), поддерживает несколько вариантов перевода (опять же, загружаемых вами, любых), и — по одной горячей клавише переключается между воспроизведением с английскими субтитрами и паузой с двуязычными субтитрами (или же вообще всё время показывает двуязычные субтитры) — так и лично мне удобнее, и если ваша цель — просто смотреть кино, иногда посматривая в перевод, так оно, по-моему, комфортнее будет :)
<a href="https://0xothik.wordpress.com/bilingual-subtitler/"><b>Подробнее о программе →</b></a>
<a href="https://github.com/0xotHik/BilingualSubtitler/releases/latest"><b>Скачать последний релиз →</b></a>

<span style="text-decoration: underline;">**Как работает программа:**</span> <span style="text-decoration: underline;">1\. Создание двуязычных субтитров:</span> - После загрузки пользователем оригинальных и нужного количества русских субтитров и выбора цветов, по кнопке "Создать субтитры" по пути в окне "Путь итоговых файлов" создаются 2 файла с **.ass**-субтитрами — оригинальными и двуязычными (или только двуязычные, если пользователь выбрал такую опцию в настройках). <span style="text-decoration: underline;">2\. Воспроизведение с динамически включаемыми двуязычными субтитрами субтитрами.</span> Пользователь стартует воспроизведение видео в видеоплеере с английскими субтитрами. Предполагается, что видеоплеер поставит первыми в списке дорожек субтитров именно созданные файлы с оригинальными и двуязычными субтитрами (так делает Media Player Classic HomeCinema) Когда в субтитрах появляется непонятный ему момент, пользователь нажимает горячую клавишу, заданную в программе [регистрирующуюся в системе через [**NonInvasiveKeyboardHook**](https://github.com/kfirprods/NonInvasiveKeyboardHook)]. В этот момент в программе проверяется совпадение имени активного процесса [получаем через функцию **GetForegroundWindow** и **GetWindowThreadProcessId** из **user32.dll**] имени процесса видеоплеера, задаваемому в настройках, и в случае совпадения — эмулируются нажатия на клавишу паузы и смены субтитров на следующие. [Эмуляция происходит либо через функцию **PostMessage** из **user32.dll** (в случае, если эмулируется нажатие одной клавиши), либо через библиотеку **WindowsInput.[InputSimulator](https://github.com/michaelnoonan/inputsimulator)** (в случае эмуляции нажатия сочетания клавиши и клавиши-модификатора Ctrl, Alt или Shift. Через InputSimulator на глаз работает помедленнее, чем через PostMessage (потому что, как я понимаю, InputSimulator полноценно эмулирует нажатие клавиш, PostMessage лишь посылает сигнал о нажатии в процесс), но я не смог через PostMessage воплотить нажатие сочетания клавиши и клавиши-модификатора).] Пользователь прочитывает варианты перевода, вновь нажимает на горячую клавишу программы — эмулируются нажатия на клавишу паузы и смены субтитров на предыдущие — вновь включаются одни английские субтитры, воспроизведение продолжается.

<span style="text-decoration: underline;">**Авторы:**</span> 
- Автор программы — Евгений <span style="color: #000080;">**0xotHik**</span> Помелов
Программа использует:
*   [Библиотеки Яндекс.Лингвистики для .Net](https://habrahabr.ru/post/204372/) авторства Ивана **KvanTTT** Кочуркина ([StackOverflow](http://stackoverflow.com/users/1046374/kvanttt), [LinkedIn](https://ru.linkedin.com/in/kvanttt/en), [GitHub](http://github.com/KvanTTT/))
*   [Библиотеку **LibSE**](https://github.com/SubtitleEdit/subtitleedit/tree/master/libse) из состава приложения [Subtitle Edit](https://0xothik.wordpress.com/bilingual-subtitler#SubtitleEdit) — с её помощью осуществляется считывание субтитров из файлов .mkv.
*   [Библиотеку **Windows Input Simulator**](https://github.com/michaelnoonan/inputsimulator) (с неправильными примерами кода в разделе Example :))
*   [Библиотеку **NonInvasiveKeyboardHook**](https://github.com/kfirprods/NonInvasiveKeyboardHook)
*   Установщик создан через [программу **Inno Setup**](https://jrsoftware.org/isinfo.php)

- Большое спасибо компании **Яндекс** за предоставление бесплатного доступа к своим API Автор будет рад [откликам о программе в любом виде](https://0xothik.wordpress.com/bilingual-subtitler#ContactMe).

