/****************************************************************************
** Meta object code from reading C++ file 'newconfig.h'
**
** Created by: The Qt Meta Object Compiler version 67 (Qt 5.9.3)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../WarnDialog/newconfig.h"
#include <QtCore/qbytearray.h>
#include <QtCore/qmetatype.h>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'newconfig.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 67
#error "This file was generated using the moc from 5.9.3. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
QT_WARNING_PUSH
QT_WARNING_DISABLE_DEPRECATED
struct qt_meta_stringdata_NewConfig_t {
    QByteArrayData data[10];
    char stringdata0[140];
};
#define QT_MOC_LITERAL(idx, ofs, len) \
    Q_STATIC_BYTE_ARRAY_DATA_HEADER_INITIALIZER_WITH_OFFSET(len, \
    qptrdiff(offsetof(qt_meta_stringdata_NewConfig_t, stringdata0) + ofs \
        - idx * sizeof(QByteArrayData)) \
    )
static const qt_meta_stringdata_NewConfig_t qt_meta_stringdata_NewConfig = {
    {
QT_MOC_LITERAL(0, 0, 9), // "NewConfig"
QT_MOC_LITERAL(1, 10, 22), // "on_CloseButton_clicked"
QT_MOC_LITERAL(2, 33, 0), // ""
QT_MOC_LITERAL(3, 34, 18), // "on_NewHong_clicked"
QT_MOC_LITERAL(4, 53, 22), // "on_lineEdit_textEdited"
QT_MOC_LITERAL(5, 76, 4), // "arg1"
QT_MOC_LITERAL(6, 81, 15), // "WindowClosedLan"
QT_MOC_LITERAL(7, 97, 20), // "on_NewHong_2_clicked"
QT_MOC_LITERAL(8, 118, 15), // "SlotAllLanguage"
QT_MOC_LITERAL(9, 134, 5) // "index"

    },
    "NewConfig\0on_CloseButton_clicked\0\0"
    "on_NewHong_clicked\0on_lineEdit_textEdited\0"
    "arg1\0WindowClosedLan\0on_NewHong_2_clicked\0"
    "SlotAllLanguage\0index"
};
#undef QT_MOC_LITERAL

static const uint qt_meta_data_NewConfig[] = {

 // content:
       7,       // revision
       0,       // classname
       0,    0, // classinfo
       6,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: name, argc, parameters, tag, flags
       1,    0,   44,    2, 0x08 /* Private */,
       3,    0,   45,    2, 0x08 /* Private */,
       4,    1,   46,    2, 0x08 /* Private */,
       6,    0,   49,    2, 0x08 /* Private */,
       7,    0,   50,    2, 0x08 /* Private */,
       8,    1,   51,    2, 0x08 /* Private */,

 // slots: parameters
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, QMetaType::QString,    5,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, QMetaType::Int,    9,

       0        // eod
};

void NewConfig::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        NewConfig *_t = static_cast<NewConfig *>(_o);
        Q_UNUSED(_t)
        switch (_id) {
        case 0: _t->on_CloseButton_clicked(); break;
        case 1: _t->on_NewHong_clicked(); break;
        case 2: _t->on_lineEdit_textEdited((*reinterpret_cast< const QString(*)>(_a[1]))); break;
        case 3: _t->WindowClosedLan(); break;
        case 4: _t->on_NewHong_2_clicked(); break;
        case 5: _t->SlotAllLanguage((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObject NewConfig::staticMetaObject = {
    { &QDialog::staticMetaObject, qt_meta_stringdata_NewConfig.data,
      qt_meta_data_NewConfig,  qt_static_metacall, nullptr, nullptr}
};


const QMetaObject *NewConfig::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *NewConfig::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_NewConfig.stringdata0))
        return static_cast<void*>(this);
    return QDialog::qt_metacast(_clname);
}

int NewConfig::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QDialog::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 6)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 6;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 6)
            *reinterpret_cast<int*>(_a[0]) = -1;
        _id -= 6;
    }
    return _id;
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
