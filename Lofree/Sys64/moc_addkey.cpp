/****************************************************************************
** Meta object code from reading C++ file 'addkey.h'
**
** Created by: The Qt Meta Object Compiler version 67 (Qt 5.9.3)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../WarnDialog/addkey.h"
#include <QtCore/qbytearray.h>
#include <QtCore/qmetatype.h>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'addkey.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 67
#error "This file was generated using the moc from 5.9.3. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
QT_WARNING_PUSH
QT_WARNING_DISABLE_DEPRECATED
struct qt_meta_stringdata_addKey_t {
    QByteArrayData data[11];
    char stringdata0[133];
};
#define QT_MOC_LITERAL(idx, ofs, len) \
    Q_STATIC_BYTE_ARRAY_DATA_HEADER_INITIALIZER_WITH_OFFSET(len, \
    qptrdiff(offsetof(qt_meta_stringdata_addKey_t, stringdata0) + ofs \
        - idx * sizeof(QByteArrayData)) \
    )
static const qt_meta_stringdata_addKey_t qt_meta_stringdata_addKey = {
    {
QT_MOC_LITERAL(0, 0, 6), // "addKey"
QT_MOC_LITERAL(1, 7, 22), // "on_CloseButton_clicked"
QT_MOC_LITERAL(2, 30, 0), // ""
QT_MOC_LITERAL(3, 31, 18), // "on_NewHong_clicked"
QT_MOC_LITERAL(4, 50, 20), // "on_NewHong_2_clicked"
QT_MOC_LITERAL(5, 71, 17), // "UpdateCapslockTip"
QT_MOC_LITERAL(6, 89, 7), // "keyCode"
QT_MOC_LITERAL(7, 97, 8), // "downOrUp"
QT_MOC_LITERAL(8, 106, 4), // "aotu"
QT_MOC_LITERAL(9, 111, 15), // "SlotAllLanguage"
QT_MOC_LITERAL(10, 127, 5) // "index"

    },
    "addKey\0on_CloseButton_clicked\0\0"
    "on_NewHong_clicked\0on_NewHong_2_clicked\0"
    "UpdateCapslockTip\0keyCode\0downOrUp\0"
    "aotu\0SlotAllLanguage\0index"
};
#undef QT_MOC_LITERAL

static const uint qt_meta_data_addKey[] = {

 // content:
       7,       // revision
       0,       // classname
       0,    0, // classinfo
       5,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: name, argc, parameters, tag, flags
       1,    0,   39,    2, 0x08 /* Private */,
       3,    0,   40,    2, 0x08 /* Private */,
       4,    0,   41,    2, 0x08 /* Private */,
       5,    3,   42,    2, 0x08 /* Private */,
       9,    1,   49,    2, 0x08 /* Private */,

 // slots: parameters
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, QMetaType::Int, QMetaType::Bool, QMetaType::Bool,    6,    7,    8,
    QMetaType::Void, QMetaType::Int,   10,

       0        // eod
};

void addKey::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        addKey *_t = static_cast<addKey *>(_o);
        Q_UNUSED(_t)
        switch (_id) {
        case 0: _t->on_CloseButton_clicked(); break;
        case 1: _t->on_NewHong_clicked(); break;
        case 2: _t->on_NewHong_2_clicked(); break;
        case 3: _t->UpdateCapslockTip((*reinterpret_cast< int(*)>(_a[1])),(*reinterpret_cast< bool(*)>(_a[2])),(*reinterpret_cast< bool(*)>(_a[3]))); break;
        case 4: _t->SlotAllLanguage((*reinterpret_cast< int(*)>(_a[1]))); break;
        default: ;
        }
    }
}

const QMetaObject addKey::staticMetaObject = {
    { &QDialog::staticMetaObject, qt_meta_stringdata_addKey.data,
      qt_meta_data_addKey,  qt_static_metacall, nullptr, nullptr}
};


const QMetaObject *addKey::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *addKey::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_addKey.stringdata0))
        return static_cast<void*>(this);
    return QDialog::qt_metacast(_clname);
}

int addKey::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QDialog::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 5)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 5;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 5)
            *reinterpret_cast<int*>(_a[0]) = -1;
        _id -= 5;
    }
    return _id;
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
