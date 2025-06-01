/****************************************************************************
** Resource object code
**
** Created by: The Resource Compiler for Qt version 5.9.3
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

static const unsigned char qt_resource_data[] = {
  // E:/Lofree Key Mapper/Lofree Key Mapper/Install Package/packages/v1.1.1.2/Code/64/etc/qt.conf
  0x0,0x0,0x0,0x2e,
  0x5b,
  0x50,0x6c,0x61,0x74,0x66,0x6f,0x72,0x6d,0x73,0x5d,0xd,0xa,0x57,0x69,0x6e,0x64,
  0x6f,0x77,0x73,0x41,0x72,0x67,0x75,0x6d,0x65,0x6e,0x74,0x73,0x20,0x3d,0x20,0x64,
  0x70,0x69,0x61,0x77,0x61,0x72,0x65,0x6e,0x65,0x73,0x73,0x3d,0x30,
  
};

static const unsigned char qt_resource_name[] = {
  // qt.conf
  0x0,0x7,
  0x8,0x74,0xa6,0xa6,
  0x0,0x71,
  0x0,0x74,0x0,0x2e,0x0,0x63,0x0,0x6f,0x0,0x6e,0x0,0x66,
  
};

static const unsigned char qt_resource_struct[] = {
  // :
  0x0,0x0,0x0,0x0,0x0,0x2,0x0,0x0,0x0,0x1,0x0,0x0,0x0,0x1,
0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,
  // :/qt.conf
  0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x0,0x1,0x0,0x0,0x0,0x0,
0x0,0x0,0x1,0x87,0x7e,0x82,0xe5,0x4,

};

#ifdef QT_NAMESPACE
#  define QT_RCC_PREPEND_NAMESPACE(name) ::QT_NAMESPACE::name
#  define QT_RCC_MANGLE_NAMESPACE0(x) x
#  define QT_RCC_MANGLE_NAMESPACE1(a, b) a##_##b
#  define QT_RCC_MANGLE_NAMESPACE2(a, b) QT_RCC_MANGLE_NAMESPACE1(a,b)
#  define QT_RCC_MANGLE_NAMESPACE(name) QT_RCC_MANGLE_NAMESPACE2( \
        QT_RCC_MANGLE_NAMESPACE0(name), QT_RCC_MANGLE_NAMESPACE0(QT_NAMESPACE))
#else
#   define QT_RCC_PREPEND_NAMESPACE(name) name
#   define QT_RCC_MANGLE_NAMESPACE(name) name
#endif

#ifdef QT_NAMESPACE
namespace QT_NAMESPACE {
#endif

bool qRegisterResourceData(int, const unsigned char *, const unsigned char *, const unsigned char *);

bool qUnregisterResourceData(int, const unsigned char *, const unsigned char *, const unsigned char *);

#ifdef QT_NAMESPACE
}
#endif

int QT_RCC_MANGLE_NAMESPACE(qInitResources_conf)();
int QT_RCC_MANGLE_NAMESPACE(qInitResources_conf)()
{
    QT_RCC_PREPEND_NAMESPACE(qRegisterResourceData)
        (0x2, qt_resource_struct, qt_resource_name, qt_resource_data);
    return 1;
}

int QT_RCC_MANGLE_NAMESPACE(qCleanupResources_conf)();
int QT_RCC_MANGLE_NAMESPACE(qCleanupResources_conf)()
{
    QT_RCC_PREPEND_NAMESPACE(qUnregisterResourceData)
       (0x2, qt_resource_struct, qt_resource_name, qt_resource_data);
    return 1;
}

namespace {
   struct initializer {
       initializer() { QT_RCC_MANGLE_NAMESPACE(qInitResources_conf)(); }
       ~initializer() { QT_RCC_MANGLE_NAMESPACE(qCleanupResources_conf)(); }
   } dummy;
}
