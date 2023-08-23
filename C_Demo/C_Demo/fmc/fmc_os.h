/*******************************************************************************
**  Copyright (C) 2015 Flaircomm Microelectronics, Inc.
**
** File Name:
**      fmc_os.h
** File Description:
**      This file contains Fmc Os Api
** File History:
**------------------------------------------------------------------------------
**  Ver   Date         Author         Description
**------------------------------------------------------------------------------
**  1.0   2020-7-9   Flaircomm      First Release
*******************************************************************************/

#ifndef __FMC_OS_H__
#define __FMC_OS_H__

/******************************************************************************/
#ifdef __cplusplus
extern "C" {
#endif


/******************************************************************************
             INCLUDE FILES SECTION
******************************************************************************/
#include "fmc_type.h"


/*******************************************************************************
             GLOBAL EXPORTED TYPE, CONSTANT, MACRO DEFINITIONS SECTION
*******************************************************************************/
#define FMC_LOG         FmcOsLog
#define FMC_LOG_HEX     FmcOsLogHex

/* Log Reasons */
typedef enum
{
    FMC_LOG_BEGIN = 0,  /* 0 */
    FMC_LOG_FATAL = FMC_LOG_BEGIN,
    FMC_LOG_CRITICAL,   /* 1 */
    FMC_LOG_ERROR,      /* 2 */
    FMC_LOG_WARNING,    /* 3 */

    FMC_LOG_ENTRY,      /* 4 */
    FMC_LOG_EXIT,       /* 5 */
    FMC_LOG_TRACE,      /* 6 */
    FMC_LOG_DEBUG,      /* 7 */

    FMC_LOG_END         /* 8 */
} FmcLogReasons1_e;

typedef UINT16      FmcLogReasons_e;


/*******************************************************************************
             GLOBAL EXPORTED VARIABLES DECLARATIONS SECTION
*******************************************************************************/


/*******************************************************************************
             GLOBAL EXPORTED FUNCTIONS DECLARATIONS SECTION
*******************************************************************************/
void FmcOsLog(const FmcLogReasons_e eReason, const FmcOsStrPtr_t pFormat, ...);
void FmcOsLogHex(const FmcLogReasons_e eReason, UINT8 *pData, UINT16 u16Len);

/*****************************************************************************/
#ifdef __cplusplus  // close out "C" linkage in case of c++ compiling
}
#endif


/******************************************************************************/
#endif /* __FMC_OS_H__ */

