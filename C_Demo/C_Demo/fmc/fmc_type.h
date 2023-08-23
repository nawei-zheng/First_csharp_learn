/*******************************************************************************
**  Copyright (C) 2015 Flaircomm Microelectronics, Inc.
**
** File Name:
**      fmc_type.h
** File Description:
**      This file contains type definitions
**      该部分是用于适配codeblocks定义，与实际MCU代码会有差异
** File History:
**------------------------------------------------------------------------------
**  Ver   Date         Author         Description
**------------------------------------------------------------------------------
**  1.0   2020-7-9   Flaircomm      First Release
*******************************************************************************/

#ifndef __FMC_TYPE_H__
#define __FMC_TYPE_H__

/******************************************************************************/
#ifdef __cplusplus
extern "C" {
#endif


/******************************************************************************
             INCLUDE FILES SECTION
******************************************************************************/
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdarg.h>

/*******************************************************************************
             GLOBAL EXPORTED TYPE, CONSTANT, MACRO DEFINITIONS SECTION
*******************************************************************************/
#define FMC_FAR

#ifndef BOOL
#define BOOL                unsigned char
#endif

#ifndef INT8
#define INT8                char
#endif

#ifndef UINT8
#define UINT8               unsigned char
#endif

#ifndef INT16
#define INT16               short
#endif

#ifndef UINT16
#define UINT16              unsigned short
#endif

#ifndef INT32
#define INT32               int
#endif

#ifndef UINT32
#define UINT32              unsigned int
#endif

#ifndef TRUE
  #define TRUE      1
#endif

#ifndef FALSE
  #define FALSE     0
#endif

#ifndef NULL
#define NULL                ((void *)0)
#endif

#define ARRAY_SIZE(a)      (sizeof(a)/sizeof((a)[0]))
#define BYTE_BIT(i)        ((UINT8)(0x01<<(i)))
#define WORD_BIT(i)        ((UINT16)(0x01u<<(i)))

typedef void                            IGNORE_TYPE;
typedef void *FMC_FAR                   FmcOsMemPtr_t;
typedef INT8                            FmcOsStrChar;
typedef FmcOsStrChar *FMC_FAR           FmcOsStrPtr_t;
typedef FmcOsStrChar const *FMC_FAR     FmcOsConstStrPtr_t;


/*******************************************************************************
             GLOBAL EXPORTED VARIABLES DECLARATIONS SECTION
*******************************************************************************/


/*******************************************************************************
             GLOBAL EXPORTED FUNCTIONS DECLARATIONS SECTION
*******************************************************************************/


/*****************************************************************************/
#ifdef __cplusplus  // close out "C" linkage in case of c++ compiling
}
#endif


/******************************************************************************/
#endif /* __FMC_TYPE_H__ */

