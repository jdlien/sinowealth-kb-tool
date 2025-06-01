/****************************************************************************
** Meta object code from reading C++ file 'formupdate.h'
**
** Created by: The Qt Meta Object Compiler version 67 (Qt 5.9.3)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../WarnDialog/formupdate.h"
#include <QtCore/qbytearray.h>
#include <QtCore/qmetatype.h>
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'formupdate.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 67
#error "This file was generated using the moc from 5.9.3. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
QT_WARNING_PUSH
QT_WARNING_DISABLE_DEPRECATED
struct qt_meta_stringdata_FormUpdate_t {
    QByteArrayData data[16];
    char stringdata0[237];
};
#define QT_MOC_LITERAL(idx, ofs, len) \
    Q_STATIC_BYTE_ARRAY_DATA_HEADER_INITIALIZER_WITH_OFFSET(len, \
    qptrdiff(offsetof(qt_meta_stringdata_FormUpdate_t, stringdata0) + ofs \
        - idx * sizeof(QByteArrayData)) \
    )
static const qt_meta_stringdata_FormUpdate_t qt_meta_stringdata_FormUpdate = {
    {
QT_MOC_LITERAL(0, 0, 10), // "FormUpdate"
QT_MOC_LITERAL(1, 11, 12), // "onupdateTime"
QT_MOC_LITERAL(2, 24, 0), // ""
QT_MOC_LITERAL(3, 25, 18), // "oncheckDevicesTime"
QT_MOC_LITERAL(4, 44, 13), // "onDongleStart"
QT_MOC_LITERAL(5, 58, 12), // "onMouseStart"
QT_MOC_LITERAL(6, 71, 12), // "onDongleStop"
QT_MOC_LITERAL(7, 84, 17), // "on_Update_clicked"
QT_MOC_LITERAL(8, 102, 19), // "SlotSetProcessValue"
QT_MOC_LITERAL(9, 122, 5), // "value"
QT_MOC_LITERAL(10, 128, 14), // "SlotFormFinish"
QT_MOC_LITERAL(11, 143, 16), // "SlotUpDateFinish"
QT_MOC_LITERAL(12, 160, 22), // "on_CloseButton_clicked"
QT_MOC_LITERAL(13, 183, 13), // "on_OK_clicked"
QT_MOC_LITERAL(14, 197, 17), // "on_Cancel_clicked"
QT_MOC_LITERAL(15, 215, 21) // "on_pushButton_clicked"

    },
    "FormUpdate\0onupdateTime\0\0oncheckDevicesTime\0"
    "onDongleStart\0onMouseStart\0onDongleStop\0"
    "on_Update_clicked\0SlotSetProcessValue\0"
    "value\0SlotFormFinish\0SlotUpDateFinish\0"
    "on_CloseButton_clicked\0on_OK_clicked\0"
    "on_Cancel_clicked\0on_pushButton_clicked"
};
#undef QT_MOC_LITERAL

static const uint qt_meta_data_FormUpdate[] = {

 // content:
       7,       // revision
       0,       // classname
       0,    0, // classinfo
      13,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: name, argc, parameters, tag, flags
       1,    0,   79,    2, 0x08 /* Private */,
       3,    0,   80,    2, 0x08 /* Private */,
       4,    0,   81,    2, 0x08 /* Private */,
       5,    0,   82,    2, 0x08 /* Private */,
       6,    0,   83,    2, 0x08 /* Private */,
       7,    0,   84,    2, 0x08 /* Private */,
       8,    1,   85,    2, 0x08 /* Private */,
      10,    0,   88,    2, 0x08 /* Private */,
      11,    0,   89,    2, 0x08 /* Private */,
      12,    0,   90,    2, 0x08 /* Private */,
      13,    0,   91,    2, 0x08 /* Private */,
      14,    0,   92,    2, 0x08 /* Private */,
      15,    0,   93,    2, 0x08 /* Private */,

 // slots: parameters
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void, QMetaType::Int,    9,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,
    QMetaType::Void,

       0        // eod
};

void FormUpdate::qt_static_metacall(QObject *_o, QMetaObject::Call _c, int _id, void **_a)
{
    if (_c == QMetaObject::InvokeMetaMethod) {
        FormUpdate *_t = static_cast<FormUpdate *>(_o);
        Q_UNUSED(_t)
        switch (_id) {
        case 0: _t->onupdateTime(); break;
        case 1: _t->oncheckDevicesTime(); break;
        case 2: _t->onDongleStart(); break;
        case 3: _t->onMouseStart(); break;
        case 4: _t->onDongleStop(); break;
        case 5: _t->on_Update_clicked(); break;
        case 6: _t->SlotSetProcessValue((*reinterpret_cast< int(*)>(_a[1]))); break;
        case 7: _t->SlotFormFinish(); break;
        case 8: _t->SlotUpDateFinish(); break;
        case 9: _t->on_CloseButton_clicked(); break;
        case 10: _t->on_OK_clicked(); break;
        case 11: _t->on_Cancel_clicked(); break;
        case 12: _t->on_pushButton_clicked(); break;
        default: ;
        }
    }
}

const QMetaObject FormUpdate::staticMetaObject = {
    { &QDialog::staticMetaObject, qt_meta_stringdata_FormUpdate.data,
      qt_meta_data_FormUpdate,  qt_static_metacall, nullptr, nullptr}
};


const QMetaObject *FormUpdate::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->dynamicMetaObject() : &staticMetaObject;
}

void *FormUpdate::qt_metacast(const char *_clname)
{
    if (!_clname) return nullptr;
    if (!strcmp(_clname, qt_meta_stringdata_FormUpdate.stringdata0))
        return static_cast<void*>(this);
    return QDialog::qt_metacast(_clname);
}

int FormUpdate::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QDialog::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        if (_id < 13)
            qt_static_metacall(this, _c, _id, _a);
        _id -= 13;
    } else if (_c == QMetaObject::RegisterMethodArgumentMetaType) {
        if (_id < 13)
            *reinterpret_cast<int*>(_a[0]) = -1;
        _id -= 13;
    }
    return _id;
}
QT_WARNING_POP
QT_END_MOC_NAMESPACE
