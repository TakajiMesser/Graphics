using SpiceEngineCore.Geometry;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SpiceEngine.GLFWBindings
{
    public static unsafe class GL
    {
        private static DEL_V_EAccumOpEF _glAccum;
        private static DEL_V_UiUi _glActiveShaderProgram;
        private static DEL_V_ETextureUnitE _glActiveTexture;
        private static DEL_V_EAlphaFunctionEF _glAlphaFunc;
        private static DEL_B_IUipBp _glAreTexturesResident;
        private static DEL_V_I _glArrayElement;
        private static DEL_V_UiUi _glAttachShader;
        private static DEL_V_EPrimitiveTypeE _glBegin;
        private static DEL_V_UiEConditionalRenderModeE _glBeginConditionalRender;
        private static DEL_V_EQueryTargetEUi _glBeginQuery;
        private static DEL_V_EQueryTargetEUiUi _glBeginQueryIndexed;
        private static DEL_V_EPrimitiveTypeE _glBeginTransformFeedback;
        private static DEL_V_UiUiCp _glBindAttribLocation;
        private static DEL_V_EBufferTargetARBEUi _glBindBuffer;
        private static DEL_V_EBufferTargetARBEUiUi _glBindBufferBase;
        private static DEL_V_EBufferTargetARBEUiUiPP _glBindBufferRange;
        private static DEL_V_EBufferTargetARBEUiIUip _glBindBuffersBase;
        private static DEL_V_EBufferTargetARBEUiIUipPpPp _glBindBuffersRange;
        private static DEL_V_UiUiCp _glBindFragDataLocation;
        private static DEL_V_UiUiUiCp _glBindFragDataLocationIndexed;
        private static DEL_V_EFramebufferTargetEUi _glBindFramebuffer;
        private static DEL_V_UiUiIBIEBufferAccessARBEEInternalFormatE _glBindImageTexture;
        private static DEL_V_UiIUip _glBindImageTextures;
        private static DEL_V_Ui _glBindProgramPipeline;
        private static DEL_V_ERenderbufferTargetEUi _glBindRenderbuffer;
        private static DEL_V_UiUi _glBindSampler;
        private static DEL_V_UiIUip _glBindSamplers;
        private static DEL_V_ETextureTargetEUi _glBindTexture;
        private static DEL_V_UiIUip _glBindTextures;
        private static DEL_V_UiUi _glBindTextureUnit;
        private static DEL_V_EBindTransformFeedbackTargetEUi _glBindTransformFeedback;
        private static DEL_V_Ui _glBindVertexArray;
        private static DEL_V_UiUiPI _glBindVertexBuffer;
        private static DEL_V_UiIUipPpIp _glBindVertexBuffers;
        private static DEL_V_IIFFFFByp _glBitmap;
        private static DEL_V_FFFF _glBlendColor;
        private static DEL_V_EBlendEquationModeEXTE _glBlendEquation;
        private static DEL_V_UiEBlendEquationModeEXTE _glBlendEquationi;
        private static DEL_V_EBlendEquationModeEXTEEBlendEquationModeEXTE _glBlendEquationSeparate;
        private static DEL_V_UiEBlendEquationModeEXTEEBlendEquationModeEXTE _glBlendEquationSeparatei;
        private static DEL_V_EBlendingFactorEEBlendingFactorE _glBlendFunc;
        private static DEL_V_UiEBlendingFactorEEBlendingFactorE _glBlendFunci;
        private static DEL_V_EBlendingFactorEEBlendingFactorEEBlendingFactorEEBlendingFactorE _glBlendFuncSeparate;
        private static DEL_V_UiEBlendingFactorEEBlendingFactorEEBlendingFactorEEBlendingFactorE _glBlendFuncSeparatei;
        private static DEL_V_IIIIIIIIEClearBufferMaskEEBlitFramebufferFilterE _glBlitFramebuffer;
        private static DEL_V_UiUiIIIIIIIIEClearBufferMaskEEBlitFramebufferFilterE _glBlitNamedFramebuffer;
        private static DEL_V_EBufferTargetARBEPVpEBufferUsageARBE _glBufferData;
        private static DEL_V_EBufferStorageTargetEPVpEBufferStorageMaskE _glBufferStorage;
        private static DEL_V_EBufferTargetARBEPPVp _glBufferSubData;
        private static DEL_V_Ui _glCallList;
        private static DEL_V_IEListNameTypeEVp _glCallLists;
        private static DEL_EFramebufferStatusE_EFramebufferTargetE _glCheckFramebufferStatus;
        private static DEL_EFramebufferStatusE_UiEFramebufferTargetE _glCheckNamedFramebufferStatus;
        private static DEL_V_EClampColorTargetARBEEClampColorModeARBE _glClampColor;
        private static DEL_V_EClearBufferMaskE _glClear;
        private static DEL_V_FFFF _glClearAccum;
        private static DEL_V_EBufferStorageTargetEEInternalFormatEEPixelFormatEEPixelTypeEVp _glClearBufferData;
        private static DEL_V_EBufferEIFI _glClearBufferfi;
        private static DEL_V_EBufferEIFp _glClearBufferfv;
        private static DEL_V_EBufferEIIp _glClearBufferiv;
        private static DEL_V_EBufferTargetARBEEInternalFormatEPPEPixelFormatEEPixelTypeEVp _glClearBufferSubData;
        private static DEL_V_EBufferEIUip _glClearBufferuiv;
        private static DEL_V_FFFF _glClearColor;
        private static DEL_V_D _glClearDepth;
        private static DEL_V_F _glClearDepthf;
        private static DEL_V_F _glClearIndex;
        private static DEL_V_UiEInternalFormatEEPixelFormatEEPixelTypeEVp _glClearNamedBufferData;
        private static DEL_V_UiEInternalFormatEPPEPixelFormatEEPixelTypeEVp _glClearNamedBufferSubData;
        private static DEL_V_UiEBufferEIFI _glClearNamedFramebufferfi;
        private static DEL_V_UiEBufferEIFp _glClearNamedFramebufferfv;
        private static DEL_V_UiEBufferEIIp _glClearNamedFramebufferiv;
        private static DEL_V_UiEBufferEIUip _glClearNamedFramebufferuiv;
        private static DEL_V_I _glClearStencil;
        private static DEL_V_UiIEPixelFormatEEPixelTypeEVp _glClearTexImage;
        private static DEL_V_UiIIIIIIIEPixelFormatEEPixelTypeEVp _glClearTexSubImage;
        private static DEL_V_ETextureUnitE _glClientActiveTexture;
        private static DEL_ESyncStatusE_StpSyncStpESyncObjectMaskEUl _glClientWaitSync;
        private static DEL_V_EClipControlOriginEEClipControlDepthE _glClipControl;
        private static DEL_V_EClipPlaneNameEDp _glClipPlane;
        private static DEL_V_ByByBy _glColor3b;
        private static DEL_V_Byp _glColor3bv;
        private static DEL_V_DDD _glColor3d;
        private static DEL_V_Dp _glColor3dv;
        private static DEL_V_FFF _glColor3f;
        private static DEL_V_Fp _glColor3fv;
        private static DEL_V_III _glColor3i;
        private static DEL_V_Ip _glColor3iv;
        private static DEL_V_SSS _glColor3s;
        private static DEL_V_Sp _glColor3sv;
        private static DEL_V_ByByBy _glColor3ub;
        private static DEL_V_Byp _glColor3ubv;
        private static DEL_V_UiUiUi _glColor3ui;
        private static DEL_V_Uip _glColor3uiv;
        private static DEL_V_UsUsUs _glColor3us;
        private static DEL_V_Usp _glColor3usv;
        private static DEL_V_ByByByBy _glColor4b;
        private static DEL_V_Byp _glColor4bv;
        private static DEL_V_DDDD _glColor4d;
        private static DEL_V_Dp _glColor4dv;
        private static DEL_V_FFFF _glColor4f;
        private static DEL_V_Fp _glColor4fv;
        private static DEL_V_IIII _glColor4i;
        private static DEL_V_Ip _glColor4iv;
        private static DEL_V_SSSS _glColor4s;
        private static DEL_V_Sp _glColor4sv;
        private static DEL_V_ByByByBy _glColor4ub;
        private static DEL_V_Byp _glColor4ubv;
        private static DEL_V_UiUiUiUi _glColor4ui;
        private static DEL_V_Uip _glColor4uiv;
        private static DEL_V_UsUsUsUs _glColor4us;
        private static DEL_V_Usp _glColor4usv;
        private static DEL_V_BBBB _glColorMask;
        private static DEL_V_UiBBBB _glColorMaski;
        private static DEL_V_EMaterialFaceEEColorMaterialParameterE _glColorMaterial;
        private static DEL_V_EColorPointerTypeEUi _glColorP3ui;
        private static DEL_V_EColorPointerTypeEUip _glColorP3uiv;
        private static DEL_V_EColorPointerTypeEUi _glColorP4ui;
        private static DEL_V_EColorPointerTypeEUip _glColorP4uiv;
        private static DEL_V_IEColorPointerTypeEIVp _glColorPointer;
        private static DEL_V_Ui _glCompileShader;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIIVp _glCompressedTexImage1D;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIIIVp _glCompressedTexImage2D;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIIIIVp _glCompressedTexImage3D;
        private static DEL_V_ETextureTargetEIIIEPixelFormatEIVp _glCompressedTexSubImage1D;
        private static DEL_V_ETextureTargetEIIIIIEPixelFormatEIVp _glCompressedTexSubImage2D;
        private static DEL_V_ETextureTargetEIIIIIIIEPixelFormatEIVp _glCompressedTexSubImage3D;
        private static DEL_V_UiIIIEPixelFormatEIVp _glCompressedTextureSubImage1D;
        private static DEL_V_UiIIIIIEPixelFormatEIVp _glCompressedTextureSubImage2D;
        private static DEL_V_UiIIIIIIIEPixelFormatEIVp _glCompressedTextureSubImage3D;
        private static DEL_V_ECopyBufferSubDataTargetEECopyBufferSubDataTargetEPPP _glCopyBufferSubData;
        private static DEL_V_UiECopyImageSubDataTargetEIIIIUiECopyImageSubDataTargetEIIIIIII _glCopyImageSubData;
        private static DEL_V_UiUiPPP _glCopyNamedBufferSubData;
        private static DEL_V_IIIIEPixelCopyTypeE _glCopyPixels;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIII _glCopyTexImage1D;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIIII _glCopyTexImage2D;
        private static DEL_V_ETextureTargetEIIIII _glCopyTexSubImage1D;
        private static DEL_V_ETextureTargetEIIIIIII _glCopyTexSubImage2D;
        private static DEL_V_ETextureTargetEIIIIIIII _glCopyTexSubImage3D;
        private static DEL_V_UiIIIII _glCopyTextureSubImage1D;
        private static DEL_V_UiIIIIIII _glCopyTextureSubImage2D;
        private static DEL_V_UiIIIIIIII _glCopyTextureSubImage3D;
        private static DEL_V_IUip _glCreateBuffers;
        private static DEL_V_IUip _glCreateFramebuffers;
        private static DEL_Ui_ _glCreateProgram;
        private static DEL_V_IUip _glCreateProgramPipelines;
        private static DEL_V_EQueryTargetEIUip _glCreateQueries;
        private static DEL_V_IUip _glCreateRenderbuffers;
        private static DEL_V_IUip _glCreateSamplers;
        private static DEL_Ui_EShaderTypeE _glCreateShader;
        private static DEL_Ui_EShaderTypeEICpp _glCreateShaderProgramv;
        private static DEL_V_ETextureTargetEIUip _glCreateTextures;
        private static DEL_V_IUip _glCreateTransformFeedbacks;
        private static DEL_V_IUip _glCreateVertexArrays;
        private static DEL_V_ECullFaceModeE _glCullFace;
        private static DEL_V_VpVp _glDebugMessageCallback;
        private static DEL_V_EDebugSourceEEDebugTypeEEDebugSeverityEIUipB _glDebugMessageControl;
        private static DEL_V_EDebugSourceEEDebugTypeEUiEDebugSeverityEICp _glDebugMessageInsert;
        private static DEL_V_IUip _glDeleteBuffers;
        private static DEL_V_IUip _glDeleteFramebuffers;
        private static DEL_V_UiI _glDeleteLists;
        private static DEL_V_Ui _glDeleteProgram;
        private static DEL_V_IUip _glDeleteProgramPipelines;
        private static DEL_V_IUip _glDeleteQueries;
        private static DEL_V_IUip _glDeleteRenderbuffers;
        private static DEL_V_IUip _glDeleteSamplers;
        private static DEL_V_Ui _glDeleteShader;
        private static DEL_V_StpSyncStp _glDeleteSync;
        private static DEL_V_IUip _glDeleteTextures;
        private static DEL_V_IUip _glDeleteTransformFeedbacks;
        private static DEL_V_IUip _glDeleteVertexArrays;
        private static DEL_V_EDepthFunctionE _glDepthFunc;
        private static DEL_V_B _glDepthMask;
        private static DEL_V_DD _glDepthRange;
        private static DEL_V_UiIDp _glDepthRangeArrayv;
        private static DEL_V_FF _glDepthRangef;
        private static DEL_V_UiDD _glDepthRangeIndexed;
        private static DEL_V_UiUi _glDetachShader;
        private static DEL_V_EEnableCapE _glDisable;
        private static DEL_V_EEnableCapE _glDisableClientState;
        private static DEL_V_EEnableCapEUi _glDisablei;
        private static DEL_V_UiUi _glDisableVertexArrayAttrib;
        private static DEL_V_Ui _glDisableVertexAttribArray;
        private static DEL_V_UiUiUi _glDispatchCompute;
        private static DEL_V_P _glDispatchComputeIndirect;
        private static DEL_V_EPrimitiveTypeEII _glDrawArrays;
        private static DEL_V_EPrimitiveTypeEVp _glDrawArraysIndirect;
        private static DEL_V_EPrimitiveTypeEIII _glDrawArraysInstanced;
        private static DEL_V_EPrimitiveTypeEIIIUi _glDrawArraysInstancedBaseInstance;
        private static DEL_V_EDrawBufferModeE _glDrawBuffer;
        private static DEL_V_IEpDrawBufferModeEp _glDrawBuffers;
        private static DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVp _glDrawElements;
        private static DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpI _glDrawElementsBaseVertex;
        private static DEL_V_EPrimitiveTypeEEDrawElementsTypeEVp _glDrawElementsIndirect;
        private static DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpI _glDrawElementsInstanced;
        private static DEL_V_EPrimitiveTypeEIEPrimitiveTypeEVpIUi _glDrawElementsInstancedBaseInstance;
        private static DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpII _glDrawElementsInstancedBaseVertex;
        private static DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpIIUi _glDrawElementsInstancedBaseVertexBaseInstance;
        private static DEL_V_IIEPixelFormatEEPixelTypeEVp _glDrawPixels;
        private static DEL_V_EPrimitiveTypeEUiUiIEDrawElementsTypeEVp _glDrawRangeElements;
        private static DEL_V_EPrimitiveTypeEUiUiIEDrawElementsTypeEVpI _glDrawRangeElementsBaseVertex;
        private static DEL_V_EPrimitiveTypeEUi _glDrawTransformFeedback;
        private static DEL_V_EPrimitiveTypeEUiI _glDrawTransformFeedbackInstanced;
        private static DEL_V_EPrimitiveTypeEUiUi _glDrawTransformFeedbackStream;
        private static DEL_V_EPrimitiveTypeEUiUiI _glDrawTransformFeedbackStreamInstanced;
        private static DEL_V_B _glEdgeFlag;
        private static DEL_V_IVp _glEdgeFlagPointer;
        private static DEL_V_Bp _glEdgeFlagv;
        private static DEL_V_EEnableCapE _glEnable;
        private static DEL_V_EEnableCapE _glEnableClientState;
        private static DEL_V_EEnableCapEUi _glEnablei;
        private static DEL_V_UiUi _glEnableVertexArrayAttrib;
        private static DEL_V_Ui _glEnableVertexAttribArray;
        private static DEL_V_ _glEnd;
        private static DEL_V_ _glEndConditionalRender;
        private static DEL_V_ _glEndList;
        private static DEL_V_EQueryTargetE _glEndQuery;
        private static DEL_V_EQueryTargetEUi _glEndQueryIndexed;
        private static DEL_V_ _glEndTransformFeedback;
        private static DEL_V_D _glEvalCoord1d;
        private static DEL_V_Dp _glEvalCoord1dv;
        private static DEL_V_F _glEvalCoord1f;
        private static DEL_V_Fp _glEvalCoord1fv;
        private static DEL_V_DD _glEvalCoord2d;
        private static DEL_V_Dp _glEvalCoord2dv;
        private static DEL_V_FF _glEvalCoord2f;
        private static DEL_V_Fp _glEvalCoord2fv;
        private static DEL_V_EMeshMode1EII _glEvalMesh1;
        private static DEL_V_EMeshMode2EIIII _glEvalMesh2;
        private static DEL_V_I _glEvalPoint1;
        private static DEL_V_II _glEvalPoint2;
        private static DEL_V_IEFeedbackTypeEFp _glFeedbackBuffer;
        private static DEL_StpSyncStp_ESyncConditionEESyncBehaviorFlagsE _glFenceSync;
        private static DEL_V_ _glFinish;
        private static DEL_V_ _glFlush;
        private static DEL_V_EBufferTargetARBEPP _glFlushMappedBufferRange;
        private static DEL_V_UiPP _glFlushMappedNamedBufferRange;
        private static DEL_V_D _glFogCoordd;
        private static DEL_V_Dp _glFogCoorddv;
        private static DEL_V_F _glFogCoordf;
        private static DEL_V_Fp _glFogCoordfv;
        private static DEL_V_EFogPointerTypeEXTEIVp _glFogCoordPointer;
        private static DEL_V_EFogParameterEF _glFogf;
        private static DEL_V_EFogParameterEFp _glFogfv;
        private static DEL_V_EFogParameterEI _glFogi;
        private static DEL_V_EFogParameterEIp _glFogiv;
        private static DEL_V_EFramebufferTargetEEFramebufferParameterNameEI _glFramebufferParameteri;
        private static DEL_V_EFramebufferTargetEEFramebufferAttachmentEERenderbufferTargetEUi _glFramebufferRenderbuffer;
        private static DEL_V_EFramebufferTargetEEFramebufferAttachmentEUiI _glFramebufferTexture;
        private static DEL_V_EFramebufferTargetEEFramebufferAttachmentEETextureTargetEUiI _glFramebufferTexture1D;
        private static DEL_V_EFramebufferTargetEEFramebufferAttachmentEETextureTargetEUiI _glFramebufferTexture2D;
        private static DEL_V_EFramebufferTargetEEFramebufferAttachmentEETextureTargetEUiII _glFramebufferTexture3D;
        private static DEL_V_EFramebufferTargetEEFramebufferAttachmentEUiII _glFramebufferTextureLayer;
        private static DEL_V_EFrontFaceDirectionE _glFrontFace;
        private static DEL_V_DDDDDD _glFrustum;
        private static DEL_V_IUip _glGenBuffers;
        private static DEL_V_ETextureTargetE _glGenerateMipmap;
        private static DEL_V_Ui _glGenerateTextureMipmap;
        private static DEL_V_IUip _glGenFramebuffers;
        private static DEL_Ui_I _glGenLists;
        private static DEL_V_IUip _glGenProgramPipelines;
        private static DEL_V_IUip _glGenQueries;
        private static DEL_V_IUip _glGenRenderbuffers;
        private static DEL_V_IUip _glGenSamplers;
        private static DEL_V_IUip _glGenTextures;
        private static DEL_V_IUip _glGenTransformFeedbacks;
        private static DEL_V_IUip _glGenVertexArrays;
        private static DEL_V_UiUiEAtomicCounterBufferPNameEIp _glGetActiveAtomicCounterBufferiv;
        private static DEL_V_UiUiIIpIpEpAttributeTypeEpCp _glGetActiveAttrib;
        private static DEL_V_UiEShaderTypeEUiIIpCp _glGetActiveSubroutineName;
        private static DEL_V_UiEShaderTypeEUiESubroutineParameterNameEIp _glGetActiveSubroutineUniformiv;
        private static DEL_V_UiEShaderTypeEUiIIpCp _glGetActiveSubroutineUniformName;
        private static DEL_V_UiUiIIpIpEpUniformTypeEpCp _glGetActiveUniform;
        private static DEL_V_UiUiEUniformBlockPNameEIp _glGetActiveUniformBlockiv;
        private static DEL_V_UiUiIIpCp _glGetActiveUniformBlockName;
        private static DEL_V_UiUiIIpCp _glGetActiveUniformName;
        private static DEL_V_UiIUipEUniformPNameEIp _glGetActiveUniformsiv;
        private static DEL_V_UiIIpUip _glGetAttachedShaders;
        private static DEL_I_UiCp _glGetAttribLocation;
        private static DEL_V_EBufferTargetARBEUiBp _glGetBooleani_v;
        private static DEL_V_EGetPNameEBp _glGetBooleanv;
        private static DEL_V_EBufferTargetARBEEBufferPNameARBELp _glGetBufferParameteri64v;
        private static DEL_V_EBufferTargetARBEEBufferPNameARBEIp _glGetBufferParameteriv;
        private static DEL_V_EBufferTargetARBEEBufferPointerNameARBEVpp _glGetBufferPointerv;
        private static DEL_V_EBufferTargetARBEPPVp _glGetBufferSubData;
        private static DEL_V_EClipPlaneNameEDp _glGetClipPlane;
        private static DEL_V_ETextureTargetEIVp _glGetCompressedTexImage;
        private static DEL_V_UiIIVp _glGetCompressedTextureImage;
        private static DEL_V_UiIIIIIIIIVp _glGetCompressedTextureSubImage;
        private static DEL_Ui_UiIEpDebugSourceEpEpDebugTypeEpUipEpDebugSeverityEpIpCp _glGetDebugMessageLog;
        private static DEL_V_EGetPNameEUiDp _glGetDoublei_v;
        private static DEL_V_EGetPNameEDp _glGetDoublev;
        private static DEL_EErrorCodeE_ _glGetError;
        private static DEL_V_EGetPNameEUiFp _glGetFloati_v;
        private static DEL_V_EGetPNameEFp _glGetFloatv;
        private static DEL_I_UiCp _glGetFragDataIndex;
        private static DEL_I_UiCp _glGetFragDataLocation;
        private static DEL_V_EFramebufferTargetEEFramebufferAttachmentEEFramebufferAttachmentParameterNameEIp _glGetFramebufferAttachmentParameteriv;
        private static DEL_V_EFramebufferTargetEEFramebufferAttachmentParameterNameEIp _glGetFramebufferParameteriv;
        private static DEL_EGraphicsResetStatusE_ _glGetGraphicsResetStatus;
        private static DEL_V_EGetPNameEUiLp _glGetInteger64i_v;
        private static DEL_V_EGetPNameELp _glGetInteger64v;
        private static DEL_V_EGetPNameEUiIp _glGetIntegeri_v;
        private static DEL_V_EGetPNameEIp _glGetIntegerv;
        private static DEL_V_ETextureTargetEEInternalFormatEEInternalFormatPNameEILp _glGetInternalformati64v;
        private static DEL_V_ETextureTargetEEInternalFormatEEInternalFormatPNameEIIp _glGetInternalformativ;
        private static DEL_V_ELightNameEELightParameterEFp _glGetLightfv;
        private static DEL_V_ELightNameEELightParameterEIp _glGetLightiv;
        private static DEL_V_EMapTargetEEGetMapQueryEDp _glGetMapdv;
        private static DEL_V_EMapTargetEEGetMapQueryEFp _glGetMapfv;
        private static DEL_V_EMapTargetEEGetMapQueryEIp _glGetMapiv;
        private static DEL_V_EMaterialFaceEEMaterialParameterEFp _glGetMaterialfv;
        private static DEL_V_EMaterialFaceEEMaterialParameterEIp _glGetMaterialiv;
        private static DEL_V_EGetMultisamplePNameNVEUiFp _glGetMultisamplefv;
        private static DEL_V_UiEBufferPNameARBELp _glGetNamedBufferParameteri64v;
        private static DEL_V_UiEBufferPNameARBEIp _glGetNamedBufferParameteriv;
        private static DEL_V_UiEBufferPointerNameARBEVpp _glGetNamedBufferPointerv;
        private static DEL_V_UiPPVp _glGetNamedBufferSubData;
        private static DEL_V_UiEFramebufferAttachmentEEFramebufferAttachmentParameterNameEIp _glGetNamedFramebufferAttachmentParameteriv;
        private static DEL_V_UiEGetFramebufferParameterEIp _glGetNamedFramebufferParameteriv;
        private static DEL_V_UiERenderbufferParameterNameEIp _glGetNamedRenderbufferParameteriv;
        private static DEL_V_EColorTableTargetEEPixelFormatEEPixelTypeEIVp _glGetnColorTable;
        private static DEL_V_ETextureTargetEIIVp _glGetnCompressedTexImage;
        private static DEL_V_EConvolutionTargetEEPixelFormatEEPixelTypeEIVp _glGetnConvolutionFilter;
        private static DEL_V_EHistogramTargetEBEPixelFormatEEPixelTypeEIVp _glGetnHistogram;
        private static DEL_V_EMapTargetEEMapQueryEIDp _glGetnMapdv;
        private static DEL_V_EMapTargetEEMapQueryEIFp _glGetnMapfv;
        private static DEL_V_EMapTargetEEMapQueryEIIp _glGetnMapiv;
        private static DEL_V_EMinmaxTargetEBEPixelFormatEEPixelTypeEIVp _glGetnMinmax;
        private static DEL_V_EPixelMapEIFp _glGetnPixelMapfv;
        private static DEL_V_EPixelMapEIUip _glGetnPixelMapuiv;
        private static DEL_V_EPixelMapEIUsp _glGetnPixelMapusv;
        private static DEL_V_IByp _glGetnPolygonStipple;
        private static DEL_V_ESeparableTargetEEPixelFormatEEPixelTypeEIVpIVpVp _glGetnSeparableFilter;
        private static DEL_V_ETextureTargetEIEPixelFormatEEPixelTypeEIVp _glGetnTexImage;
        private static DEL_V_UiIIDp _glGetnUniformdv;
        private static DEL_V_UiIIFp _glGetnUniformfv;
        private static DEL_V_UiIIIp _glGetnUniformiv;
        private static DEL_V_UiIIUip _glGetnUniformuiv;
        private static DEL_V_EObjectIdentifierEUiIIpCp _glGetObjectLabel;
        private static DEL_V_VpIIpCp _glGetObjectPtrLabel;
        private static DEL_V_EPixelMapEFp _glGetPixelMapfv;
        private static DEL_V_EPixelMapEUip _glGetPixelMapuiv;
        private static DEL_V_EPixelMapEUsp _glGetPixelMapusv;
        private static DEL_V_EGetPointervPNameEVpp _glGetPointerv;
        private static DEL_V_Byp _glGetPolygonStipple;
        private static DEL_V_UiIIpIpVp _glGetProgramBinary;
        private static DEL_V_UiIIpCp _glGetProgramInfoLog;
        private static DEL_V_UiEProgramInterfaceEEProgramInterfacePNameEIp _glGetProgramInterfaceiv;
        private static DEL_V_UiEProgramPropertyARBEIp _glGetProgramiv;
        private static DEL_V_UiIIpCp _glGetProgramPipelineInfoLog;
        private static DEL_V_UiEPipelineParameterNameEIp _glGetProgramPipelineiv;
        private static DEL_Ui_UiEProgramInterfaceECp _glGetProgramResourceIndex;
        private static DEL_V_UiEProgramInterfaceEUiIEpProgramResourcePropertyEpIIpIp _glGetProgramResourceiv;
        private static DEL_I_UiEProgramInterfaceECp _glGetProgramResourceLocation;
        private static DEL_I_UiEProgramInterfaceECp _glGetProgramResourceLocationIndex;
        private static DEL_V_UiEProgramInterfaceEUiIIpCp _glGetProgramResourceName;
        private static DEL_V_UiEShaderTypeEEProgramStagePNameEIp _glGetProgramStageiv;
        private static DEL_V_UiUiEQueryObjectParameterNameEP _glGetQueryBufferObjecti64v;
        private static DEL_V_UiUiEQueryObjectParameterNameEP _glGetQueryBufferObjectiv;
        private static DEL_V_UiUiEQueryObjectParameterNameEP _glGetQueryBufferObjectui64v;
        private static DEL_V_UiUiEQueryObjectParameterNameEP _glGetQueryBufferObjectuiv;
        private static DEL_V_EQueryTargetEUiEQueryParameterNameEIp _glGetQueryIndexediv;
        private static DEL_V_EQueryTargetEEQueryParameterNameEIp _glGetQueryiv;
        private static DEL_V_UiEQueryObjectParameterNameELp _glGetQueryObjecti64v;
        private static DEL_V_UiEQueryObjectParameterNameEIp _glGetQueryObjectiv;
        private static DEL_V_UiEQueryObjectParameterNameEUlp _glGetQueryObjectui64v;
        private static DEL_V_UiEQueryObjectParameterNameEUip _glGetQueryObjectuiv;
        private static DEL_V_ERenderbufferTargetEERenderbufferParameterNameEIp _glGetRenderbufferParameteriv;
        private static DEL_V_UiESamplerParameterFEFp _glGetSamplerParameterfv;
        private static DEL_V_UiESamplerParameterIEIp _glGetSamplerParameterIiv;
        private static DEL_V_UiESamplerParameterIEUip _glGetSamplerParameterIuiv;
        private static DEL_V_UiESamplerParameterIEIp _glGetSamplerParameteriv;
        private static DEL_V_UiIIpCp _glGetShaderInfoLog;
        private static DEL_V_UiEShaderParameterNameEIp _glGetShaderiv;
        private static DEL_V_EShaderTypeEEPrecisionTypeEIpIp _glGetShaderPrecisionFormat;
        private static DEL_V_UiIIpCp _glGetShaderSource;
        private static DEL_Byp_EStringNameE _glGetString;
        private static DEL_Byp_EStringNameEUi _glGetStringi;
        private static DEL_Ui_UiEShaderTypeECp _glGetSubroutineIndex;
        private static DEL_I_UiEShaderTypeECp _glGetSubroutineUniformLocation;
        private static DEL_V_StpSyncStpESyncParameterNameEIIpIp _glGetSynciv;
        private static DEL_V_ETextureEnvTargetEETextureEnvParameterEFp _glGetTexEnvfv;
        private static DEL_V_ETextureEnvTargetEETextureEnvParameterEIp _glGetTexEnviv;
        private static DEL_V_ETextureCoordNameEETextureGenParameterEDp _glGetTexGendv;
        private static DEL_V_ETextureCoordNameEETextureGenParameterEFp _glGetTexGenfv;
        private static DEL_V_ETextureCoordNameEETextureGenParameterEIp _glGetTexGeniv;
        private static DEL_V_ETextureTargetEIEPixelFormatEEPixelTypeEVp _glGetTexImage;
        private static DEL_V_ETextureTargetEIEGetTextureParameterEFp _glGetTexLevelParameterfv;
        private static DEL_V_ETextureTargetEIEGetTextureParameterEIp _glGetTexLevelParameteriv;
        private static DEL_V_ETextureTargetEEGetTextureParameterEFp _glGetTexParameterfv;
        private static DEL_V_ETextureTargetEEGetTextureParameterEIp _glGetTexParameterIiv;
        private static DEL_V_ETextureTargetEEGetTextureParameterEUip _glGetTexParameterIuiv;
        private static DEL_V_ETextureTargetEEGetTextureParameterEIp _glGetTexParameteriv;
        private static DEL_V_UiIEPixelFormatEEPixelTypeEIVp _glGetTextureImage;
        private static DEL_V_UiIEGetTextureParameterEFp _glGetTextureLevelParameterfv;
        private static DEL_V_UiIEGetTextureParameterEIp _glGetTextureLevelParameteriv;
        private static DEL_V_UiEGetTextureParameterEFp _glGetTextureParameterfv;
        private static DEL_V_UiEGetTextureParameterEIp _glGetTextureParameterIiv;
        private static DEL_V_UiEGetTextureParameterEUip _glGetTextureParameterIuiv;
        private static DEL_V_UiEGetTextureParameterEIp _glGetTextureParameteriv;
        private static DEL_V_UiIIIIIIIEPixelFormatEEPixelTypeEIVp _glGetTextureSubImage;
        private static DEL_V_UiETransformFeedbackPNameEUiIp _glGetTransformFeedbacki_v;
        private static DEL_V_UiETransformFeedbackPNameEUiLp _glGetTransformFeedbacki64_v;
        private static DEL_V_UiETransformFeedbackPNameEIp _glGetTransformFeedbackiv;
        private static DEL_V_UiUiIIpIpEpAttributeTypeEpCp _glGetTransformFeedbackVarying;
        private static DEL_Ui_UiCp _glGetUniformBlockIndex;
        private static DEL_V_UiIDp _glGetUniformdv;
        private static DEL_V_UiIFp _glGetUniformfv;
        private static DEL_V_UiICppUip _glGetUniformIndices;
        private static DEL_V_UiIIp _glGetUniformiv;
        private static DEL_I_UiCp _glGetUniformLocation;
        private static DEL_V_EShaderTypeEIUip _glGetUniformSubroutineuiv;
        private static DEL_V_UiIUip _glGetUniformuiv;
        private static DEL_V_UiUiEVertexArrayPNameELp _glGetVertexArrayIndexed64iv;
        private static DEL_V_UiUiEVertexArrayPNameEIp _glGetVertexArrayIndexediv;
        private static DEL_V_UiEVertexArrayPNameEIp _glGetVertexArrayiv;
        private static DEL_V_UiEVertexAttribPropertyARBEDp _glGetVertexAttribdv;
        private static DEL_V_UiEVertexAttribPropertyARBEFp _glGetVertexAttribfv;
        private static DEL_V_UiEVertexAttribEnumEIp _glGetVertexAttribIiv;
        private static DEL_V_UiEVertexAttribEnumEUip _glGetVertexAttribIuiv;
        private static DEL_V_UiEVertexAttribPropertyARBEIp _glGetVertexAttribiv;
        private static DEL_V_UiEVertexAttribEnumEDp _glGetVertexAttribLdv;
        private static DEL_V_UiEVertexAttribPointerPropertyARBEVpp _glGetVertexAttribPointerv;
        private static DEL_V_EHintTargetEEHintModeE _glHint;
        private static DEL_V_D _glIndexd;
        private static DEL_V_Dp _glIndexdv;
        private static DEL_V_F _glIndexf;
        private static DEL_V_Fp _glIndexfv;
        private static DEL_V_I _glIndexi;
        private static DEL_V_Ip _glIndexiv;
        private static DEL_V_Ui _glIndexMask;
        private static DEL_V_EIndexPointerTypeEIVp _glIndexPointer;
        private static DEL_V_S _glIndexs;
        private static DEL_V_Sp _glIndexsv;
        private static DEL_V_By _glIndexub;
        private static DEL_V_Byp _glIndexubv;
        private static DEL_V_ _glInitNames;
        private static DEL_V_EInterleavedArrayFormatEIVp _glInterleavedArrays;
        private static DEL_V_Ui _glInvalidateBufferData;
        private static DEL_V_UiPP _glInvalidateBufferSubData;
        private static DEL_V_EFramebufferTargetEIEpInvalidateFramebufferAttachmentEp _glInvalidateFramebuffer;
        private static DEL_V_UiIEpFramebufferAttachmentEp _glInvalidateNamedFramebufferData;
        private static DEL_V_UiIEpFramebufferAttachmentEpIIII _glInvalidateNamedFramebufferSubData;
        private static DEL_V_EFramebufferTargetEIEpInvalidateFramebufferAttachmentEpIIII _glInvalidateSubFramebuffer;
        private static DEL_V_UiI _glInvalidateTexImage;
        private static DEL_V_UiIIIIIII _glInvalidateTexSubImage;
        private static DEL_B_Ui _glIsBuffer;
        private static DEL_B_EEnableCapE _glIsEnabled;
        private static DEL_B_EEnableCapEUi _glIsEnabledi;
        private static DEL_B_Ui _glIsFramebuffer;
        private static DEL_B_Ui _glIsList;
        private static DEL_B_Ui _glIsProgram;
        private static DEL_B_Ui _glIsProgramPipeline;
        private static DEL_B_Ui _glIsQuery;
        private static DEL_B_Ui _glIsRenderbuffer;
        private static DEL_B_Ui _glIsSampler;
        private static DEL_B_Ui _glIsShader;
        private static DEL_B_StpSyncStp _glIsSync;
        private static DEL_B_Ui _glIsTexture;
        private static DEL_B_Ui _glIsTransformFeedback;
        private static DEL_B_Ui _glIsVertexArray;
        private static DEL_V_ELightNameEELightParameterEF _glLightf;
        private static DEL_V_ELightNameEELightParameterEFp _glLightfv;
        private static DEL_V_ELightNameEELightParameterEI _glLighti;
        private static DEL_V_ELightNameEELightParameterEIp _glLightiv;
        private static DEL_V_ELightModelParameterEF _glLightModelf;
        private static DEL_V_ELightModelParameterEFp _glLightModelfv;
        private static DEL_V_ELightModelParameterEI _glLightModeli;
        private static DEL_V_ELightModelParameterEIp _glLightModeliv;
        private static DEL_V_IUs _glLineStipple;
        private static DEL_V_F _glLineWidth;
        private static DEL_V_Ui _glLinkProgram;
        private static DEL_V_Ui _glListBase;
        private static DEL_V_ _glLoadIdentity;
        private static DEL_V_Dp _glLoadMatrixd;
        private static DEL_V_Fp _glLoadMatrixf;
        private static DEL_V_Ui _glLoadName;
        private static DEL_V_Dp _glLoadTransposeMatrixd;
        private static DEL_V_Fp _glLoadTransposeMatrixf;
        private static DEL_V_ELogicOpE _glLogicOp;
        private static DEL_V_EMapTargetEDDIIDp _glMap1d;
        private static DEL_V_EMapTargetEFFIIFp _glMap1f;
        private static DEL_V_EMapTargetEDDIIDDIIDp _glMap2d;
        private static DEL_V_EMapTargetEFFIIFFIIFp _glMap2f;
        private static DEL_Vp_EBufferTargetARBEEBufferAccessARBE _glMapBuffer;
        private static DEL_Vp_EBufferTargetARBEPPEMapBufferAccessMaskE _glMapBufferRange;
        private static DEL_V_IDD _glMapGrid1d;
        private static DEL_V_IFF _glMapGrid1f;
        private static DEL_V_IDDIDD _glMapGrid2d;
        private static DEL_V_IFFIFF _glMapGrid2f;
        private static DEL_Vp_UiEBufferAccessARBE _glMapNamedBuffer;
        private static DEL_Vp_UiPPEMapBufferAccessMaskE _glMapNamedBufferRange;
        private static DEL_V_EMaterialFaceEEMaterialParameterEF _glMaterialf;
        private static DEL_V_EMaterialFaceEEMaterialParameterEFp _glMaterialfv;
        private static DEL_V_EMaterialFaceEEMaterialParameterEI _glMateriali;
        private static DEL_V_EMaterialFaceEEMaterialParameterEIp _glMaterialiv;
        private static DEL_V_EMatrixModeE _glMatrixMode;
        private static DEL_V_EMemoryBarrierMaskE _glMemoryBarrier;
        private static DEL_V_EMemoryBarrierMaskE _glMemoryBarrierByRegion;
        private static DEL_V_F _glMinSampleShading;
        private static DEL_V_EPrimitiveTypeEIpIpI _glMultiDrawArrays;
        private static DEL_V_EPrimitiveTypeEVpII _glMultiDrawArraysIndirect;
        private static DEL_V_EPrimitiveTypeEVpPII _glMultiDrawArraysIndirectCount;
        private static DEL_V_EPrimitiveTypeEIpEDrawElementsTypeEVppI _glMultiDrawElements;
        private static DEL_V_EPrimitiveTypeEIpEDrawElementsTypeEVppIIp _glMultiDrawElementsBaseVertex;
        private static DEL_V_EPrimitiveTypeEEDrawElementsTypeEVpII _glMultiDrawElementsIndirect;
        private static DEL_V_EPrimitiveTypeEEDrawElementsTypeEVpPII _glMultiDrawElementsIndirectCount;
        private static DEL_V_ETextureUnitED _glMultiTexCoord1d;
        private static DEL_V_ETextureUnitEDp _glMultiTexCoord1dv;
        private static DEL_V_ETextureUnitEF _glMultiTexCoord1f;
        private static DEL_V_ETextureUnitEFp _glMultiTexCoord1fv;
        private static DEL_V_ETextureUnitEI _glMultiTexCoord1i;
        private static DEL_V_ETextureUnitEIp _glMultiTexCoord1iv;
        private static DEL_V_ETextureUnitES _glMultiTexCoord1s;
        private static DEL_V_ETextureUnitESp _glMultiTexCoord1sv;
        private static DEL_V_ETextureUnitEDD _glMultiTexCoord2d;
        private static DEL_V_ETextureUnitEDp _glMultiTexCoord2dv;
        private static DEL_V_ETextureUnitEFF _glMultiTexCoord2f;
        private static DEL_V_ETextureUnitEFp _glMultiTexCoord2fv;
        private static DEL_V_ETextureUnitEII _glMultiTexCoord2i;
        private static DEL_V_ETextureUnitEIp _glMultiTexCoord2iv;
        private static DEL_V_ETextureUnitESS _glMultiTexCoord2s;
        private static DEL_V_ETextureUnitESp _glMultiTexCoord2sv;
        private static DEL_V_ETextureUnitEDDD _glMultiTexCoord3d;
        private static DEL_V_ETextureUnitEDp _glMultiTexCoord3dv;
        private static DEL_V_ETextureUnitEFFF _glMultiTexCoord3f;
        private static DEL_V_ETextureUnitEFp _glMultiTexCoord3fv;
        private static DEL_V_ETextureUnitEIII _glMultiTexCoord3i;
        private static DEL_V_ETextureUnitEIp _glMultiTexCoord3iv;
        private static DEL_V_ETextureUnitESSS _glMultiTexCoord3s;
        private static DEL_V_ETextureUnitESp _glMultiTexCoord3sv;
        private static DEL_V_ETextureUnitEDDDD _glMultiTexCoord4d;
        private static DEL_V_ETextureUnitEDp _glMultiTexCoord4dv;
        private static DEL_V_ETextureUnitEFFFF _glMultiTexCoord4f;
        private static DEL_V_ETextureUnitEFp _glMultiTexCoord4fv;
        private static DEL_V_ETextureUnitEIIII _glMultiTexCoord4i;
        private static DEL_V_ETextureUnitEIp _glMultiTexCoord4iv;
        private static DEL_V_ETextureUnitESSSS _glMultiTexCoord4s;
        private static DEL_V_ETextureUnitESp _glMultiTexCoord4sv;
        private static DEL_V_ETextureUnitEETexCoordPointerTypeEUi _glMultiTexCoordP1ui;
        private static DEL_V_ETextureUnitEETexCoordPointerTypeEUip _glMultiTexCoordP1uiv;
        private static DEL_V_ETextureUnitEETexCoordPointerTypeEUi _glMultiTexCoordP2ui;
        private static DEL_V_ETextureUnitEETexCoordPointerTypeEUip _glMultiTexCoordP2uiv;
        private static DEL_V_ETextureUnitEETexCoordPointerTypeEUi _glMultiTexCoordP3ui;
        private static DEL_V_ETextureUnitEETexCoordPointerTypeEUip _glMultiTexCoordP3uiv;
        private static DEL_V_ETextureUnitEETexCoordPointerTypeEUi _glMultiTexCoordP4ui;
        private static DEL_V_ETextureUnitEETexCoordPointerTypeEUip _glMultiTexCoordP4uiv;
        private static DEL_V_Dp _glMultMatrixd;
        private static DEL_V_Fp _glMultMatrixf;
        private static DEL_V_Dp _glMultTransposeMatrixd;
        private static DEL_V_Fp _glMultTransposeMatrixf;
        private static DEL_V_UiPVpEVertexBufferObjectUsageE _glNamedBufferData;
        private static DEL_V_UiPVpEBufferStorageMaskE _glNamedBufferStorage;
        private static DEL_V_UiPPVp _glNamedBufferSubData;
        private static DEL_V_UiEColorBufferE _glNamedFramebufferDrawBuffer;
        private static DEL_V_UiIEpColorBufferEp _glNamedFramebufferDrawBuffers;
        private static DEL_V_UiEFramebufferParameterNameEI _glNamedFramebufferParameteri;
        private static DEL_V_UiEColorBufferE _glNamedFramebufferReadBuffer;
        private static DEL_V_UiEFramebufferAttachmentEERenderbufferTargetEUi _glNamedFramebufferRenderbuffer;
        private static DEL_V_UiEFramebufferAttachmentEUiI _glNamedFramebufferTexture;
        private static DEL_V_UiEFramebufferAttachmentEUiII _glNamedFramebufferTextureLayer;
        private static DEL_V_UiEInternalFormatEII _glNamedRenderbufferStorage;
        private static DEL_V_UiIEInternalFormatEII _glNamedRenderbufferStorageMultisample;
        private static DEL_V_UiEListModeE _glNewList;
        private static DEL_V_ByByBy _glNormal3b;
        private static DEL_V_Byp _glNormal3bv;
        private static DEL_V_DDD _glNormal3d;
        private static DEL_V_Dp _glNormal3dv;
        private static DEL_V_FFF _glNormal3f;
        private static DEL_V_Fp _glNormal3fv;
        private static DEL_V_III _glNormal3i;
        private static DEL_V_Ip _glNormal3iv;
        private static DEL_V_SSS _glNormal3s;
        private static DEL_V_Sp _glNormal3sv;
        private static DEL_V_ENormalPointerTypeEUi _glNormalP3ui;
        private static DEL_V_ENormalPointerTypeEUip _glNormalP3uiv;
        private static DEL_V_ENormalPointerTypeEIVp _glNormalPointer;
        private static DEL_V_EObjectIdentifierEUiICp _glObjectLabel;
        private static DEL_V_VpICp _glObjectPtrLabel;
        private static DEL_V_DDDDDD _glOrtho;
        private static DEL_V_F _glPassThrough;
        private static DEL_V_EPatchParameterNameEFp _glPatchParameterfv;
        private static DEL_V_EPatchParameterNameEI _glPatchParameteri;
        private static DEL_V_ _glPauseTransformFeedback;
        private static DEL_V_EPixelMapEIFp _glPixelMapfv;
        private static DEL_V_EPixelMapEIUip _glPixelMapuiv;
        private static DEL_V_EPixelMapEIUsp _glPixelMapusv;
        private static DEL_V_EPixelStoreParameterEF _glPixelStoref;
        private static DEL_V_EPixelStoreParameterEI _glPixelStorei;
        private static DEL_V_EPixelTransferParameterEF _glPixelTransferf;
        private static DEL_V_EPixelTransferParameterEI _glPixelTransferi;
        private static DEL_V_FF _glPixelZoom;
        private static DEL_V_EPointParameterNameARBEF _glPointParameterf;
        private static DEL_V_EPointParameterNameARBEFp _glPointParameterfv;
        private static DEL_V_EPointParameterNameARBEI _glPointParameteri;
        private static DEL_V_EPointParameterNameARBEIp _glPointParameteriv;
        private static DEL_V_F _glPointSize;
        private static DEL_V_EMaterialFaceEEPolygonModeE _glPolygonMode;
        private static DEL_V_FF _glPolygonOffset;
        private static DEL_V_FFF _glPolygonOffsetClamp;
        private static DEL_V_Byp _glPolygonStipple;
        private static DEL_V_ _glPopAttrib;
        private static DEL_V_ _glPopClientAttrib;
        private static DEL_V_ _glPopDebugGroup;
        private static DEL_V_ _glPopMatrix;
        private static DEL_V_ _glPopName;
        private static DEL_V_Ui _glPrimitiveRestartIndex;
        private static DEL_V_IUipFp _glPrioritizeTextures;
        private static DEL_V_UiIVpI _glProgramBinary;
        private static DEL_V_UiEProgramParameterPNameEI _glProgramParameteri;
        private static DEL_V_UiID _glProgramUniform1d;
        private static DEL_V_UiIIDp _glProgramUniform1dv;
        private static DEL_V_UiIF _glProgramUniform1f;
        private static DEL_V_UiIIFp _glProgramUniform1fv;
        private static DEL_V_UiII _glProgramUniform1i;
        private static DEL_V_UiIIIp _glProgramUniform1iv;
        private static DEL_V_UiIUi _glProgramUniform1ui;
        private static DEL_V_UiIIUip _glProgramUniform1uiv;
        private static DEL_V_UiIDD _glProgramUniform2d;
        private static DEL_V_UiIIDp _glProgramUniform2dv;
        private static DEL_V_UiIFF _glProgramUniform2f;
        private static DEL_V_UiIIFp _glProgramUniform2fv;
        private static DEL_V_UiIII _glProgramUniform2i;
        private static DEL_V_UiIIIp _glProgramUniform2iv;
        private static DEL_V_UiIUiUi _glProgramUniform2ui;
        private static DEL_V_UiIIUip _glProgramUniform2uiv;
        private static DEL_V_UiIDDD _glProgramUniform3d;
        private static DEL_V_UiIIDp _glProgramUniform3dv;
        private static DEL_V_UiIFFF _glProgramUniform3f;
        private static DEL_V_UiIIFp _glProgramUniform3fv;
        private static DEL_V_UiIIII _glProgramUniform3i;
        private static DEL_V_UiIIIp _glProgramUniform3iv;
        private static DEL_V_UiIUiUiUi _glProgramUniform3ui;
        private static DEL_V_UiIIUip _glProgramUniform3uiv;
        private static DEL_V_UiIDDDD _glProgramUniform4d;
        private static DEL_V_UiIIDp _glProgramUniform4dv;
        private static DEL_V_UiIFFFF _glProgramUniform4f;
        private static DEL_V_UiIIFp _glProgramUniform4fv;
        private static DEL_V_UiIIIII _glProgramUniform4i;
        private static DEL_V_UiIIIp _glProgramUniform4iv;
        private static DEL_V_UiIUiUiUiUi _glProgramUniform4ui;
        private static DEL_V_UiIIUip _glProgramUniform4uiv;
        private static DEL_V_UiIIBDp _glProgramUniformMatrix2dv;
        private static DEL_V_UiIIBFp _glProgramUniformMatrix2fv;
        private static DEL_V_UiIIBDp _glProgramUniformMatrix2x3dv;
        private static DEL_V_UiIIBFp _glProgramUniformMatrix2x3fv;
        private static DEL_V_UiIIBDp _glProgramUniformMatrix2x4dv;
        private static DEL_V_UiIIBFp _glProgramUniformMatrix2x4fv;
        private static DEL_V_UiIIBDp _glProgramUniformMatrix3dv;
        private static DEL_V_UiIIBFp _glProgramUniformMatrix3fv;
        private static DEL_V_UiIIBDp _glProgramUniformMatrix3x2dv;
        private static DEL_V_UiIIBFp _glProgramUniformMatrix3x2fv;
        private static DEL_V_UiIIBDp _glProgramUniformMatrix3x4dv;
        private static DEL_V_UiIIBFp _glProgramUniformMatrix3x4fv;
        private static DEL_V_UiIIBDp _glProgramUniformMatrix4dv;
        private static DEL_V_UiIIBFp _glProgramUniformMatrix4fv;
        private static DEL_V_UiIIBDp _glProgramUniformMatrix4x2dv;
        private static DEL_V_UiIIBFp _glProgramUniformMatrix4x2fv;
        private static DEL_V_UiIIBDp _glProgramUniformMatrix4x3dv;
        private static DEL_V_UiIIBFp _glProgramUniformMatrix4x3fv;
        private static DEL_V_EVertexProvokingModeE _glProvokingVertex;
        private static DEL_V_EAttribMaskE _glPushAttrib;
        private static DEL_V_EClientAttribMaskE _glPushClientAttrib;
        private static DEL_V_EDebugSourceEUiICp _glPushDebugGroup;
        private static DEL_V_ _glPushMatrix;
        private static DEL_V_Ui _glPushName;
        private static DEL_V_UiEQueryCounterTargetE _glQueryCounter;
        private static DEL_V_DD _glRasterPos2d;
        private static DEL_V_Dp _glRasterPos2dv;
        private static DEL_V_FF _glRasterPos2f;
        private static DEL_V_Fp _glRasterPos2fv;
        private static DEL_V_II _glRasterPos2i;
        private static DEL_V_Ip _glRasterPos2iv;
        private static DEL_V_SS _glRasterPos2s;
        private static DEL_V_Sp _glRasterPos2sv;
        private static DEL_V_DDD _glRasterPos3d;
        private static DEL_V_Dp _glRasterPos3dv;
        private static DEL_V_FFF _glRasterPos3f;
        private static DEL_V_Fp _glRasterPos3fv;
        private static DEL_V_III _glRasterPos3i;
        private static DEL_V_Ip _glRasterPos3iv;
        private static DEL_V_SSS _glRasterPos3s;
        private static DEL_V_Sp _glRasterPos3sv;
        private static DEL_V_DDDD _glRasterPos4d;
        private static DEL_V_Dp _glRasterPos4dv;
        private static DEL_V_FFFF _glRasterPos4f;
        private static DEL_V_Fp _glRasterPos4fv;
        private static DEL_V_IIII _glRasterPos4i;
        private static DEL_V_Ip _glRasterPos4iv;
        private static DEL_V_SSSS _glRasterPos4s;
        private static DEL_V_Sp _glRasterPos4sv;
        private static DEL_V_EReadBufferModeE _glReadBuffer;
        private static DEL_V_IIIIEPixelFormatEEPixelTypeEIVp _glReadnPixels;
        private static DEL_V_IIIIEPixelFormatEEPixelTypeEVp _glReadPixels;
        private static DEL_V_DDDD _glRectd;
        private static DEL_V_DpDp _glRectdv;
        private static DEL_V_FFFF _glRectf;
        private static DEL_V_FpFp _glRectfv;
        private static DEL_V_IIII _glRecti;
        private static DEL_V_IpIp _glRectiv;
        private static DEL_V_SSSS _glRects;
        private static DEL_V_SpSp _glRectsv;
        private static DEL_V_ _glReleaseShaderCompiler;
        private static DEL_V_ERenderbufferTargetEEInternalFormatEII _glRenderbufferStorage;
        private static DEL_V_ERenderbufferTargetEIEInternalFormatEII _glRenderbufferStorageMultisample;
        private static DEL_I_ERenderingModeE _glRenderMode;
        private static DEL_V_ _glResumeTransformFeedback;
        private static DEL_V_DDDD _glRotated;
        private static DEL_V_FFFF _glRotatef;
        private static DEL_V_FB _glSampleCoverage;
        private static DEL_V_UiI _glSampleMaski;
        private static DEL_V_UiESamplerParameterFEF _glSamplerParameterf;
        private static DEL_V_UiESamplerParameterFEFp _glSamplerParameterfv;
        private static DEL_V_UiESamplerParameterIEI _glSamplerParameteri;
        private static DEL_V_UiESamplerParameterIEIp _glSamplerParameterIiv;
        private static DEL_V_UiESamplerParameterIEUip _glSamplerParameterIuiv;
        private static DEL_V_UiESamplerParameterIEIp _glSamplerParameteriv;
        private static DEL_V_DDD _glScaled;
        private static DEL_V_FFF _glScalef;
        private static DEL_V_IIII _glScissor;
        private static DEL_V_UiIIp _glScissorArrayv;
        private static DEL_V_UiIIII _glScissorIndexed;
        private static DEL_V_UiIp _glScissorIndexedv;
        private static DEL_V_ByByBy _glSecondaryColor3b;
        private static DEL_V_Byp _glSecondaryColor3bv;
        private static DEL_V_DDD _glSecondaryColor3d;
        private static DEL_V_Dp _glSecondaryColor3dv;
        private static DEL_V_FFF _glSecondaryColor3f;
        private static DEL_V_Fp _glSecondaryColor3fv;
        private static DEL_V_III _glSecondaryColor3i;
        private static DEL_V_Ip _glSecondaryColor3iv;
        private static DEL_V_SSS _glSecondaryColor3s;
        private static DEL_V_Sp _glSecondaryColor3sv;
        private static DEL_V_ByByBy _glSecondaryColor3ub;
        private static DEL_V_Byp _glSecondaryColor3ubv;
        private static DEL_V_UiUiUi _glSecondaryColor3ui;
        private static DEL_V_Uip _glSecondaryColor3uiv;
        private static DEL_V_UsUsUs _glSecondaryColor3us;
        private static DEL_V_Usp _glSecondaryColor3usv;
        private static DEL_V_EColorPointerTypeEUi _glSecondaryColorP3ui;
        private static DEL_V_EColorPointerTypeEUip _glSecondaryColorP3uiv;
        private static DEL_V_IEColorPointerTypeEIVp _glSecondaryColorPointer;
        private static DEL_V_IUip _glSelectBuffer;
        private static DEL_V_EShadingModelE _glShadeModel;
        private static DEL_V_IUipEShaderBinaryFormatEVpI _glShaderBinary;
        private static DEL_V_UiICppIp _glShaderSource;
        private static DEL_V_UiUiUi _glShaderStorageBlockBinding;
        private static DEL_V_UiCpUiUipUip _glSpecializeShader;
        private static DEL_V_EStencilFunctionEIUi _glStencilFunc;
        private static DEL_V_EStencilFaceDirectionEEStencilFunctionEIUi _glStencilFuncSeparate;
        private static DEL_V_Ui _glStencilMask;
        private static DEL_V_EStencilFaceDirectionEUi _glStencilMaskSeparate;
        private static DEL_V_EStencilOpEEStencilOpEEStencilOpE _glStencilOp;
        private static DEL_V_EStencilFaceDirectionEEStencilOpEEStencilOpEEStencilOpE _glStencilOpSeparate;
        private static DEL_V_ETextureTargetEEInternalFormatEUi _glTexBuffer;
        private static DEL_V_ETextureTargetEEInternalFormatEUiPP _glTexBufferRange;
        private static DEL_V_D _glTexCoord1d;
        private static DEL_V_Dp _glTexCoord1dv;
        private static DEL_V_F _glTexCoord1f;
        private static DEL_V_Fp _glTexCoord1fv;
        private static DEL_V_I _glTexCoord1i;
        private static DEL_V_Ip _glTexCoord1iv;
        private static DEL_V_S _glTexCoord1s;
        private static DEL_V_Sp _glTexCoord1sv;
        private static DEL_V_DD _glTexCoord2d;
        private static DEL_V_Dp _glTexCoord2dv;
        private static DEL_V_FF _glTexCoord2f;
        private static DEL_V_Fp _glTexCoord2fv;
        private static DEL_V_II _glTexCoord2i;
        private static DEL_V_Ip _glTexCoord2iv;
        private static DEL_V_SS _glTexCoord2s;
        private static DEL_V_Sp _glTexCoord2sv;
        private static DEL_V_DDD _glTexCoord3d;
        private static DEL_V_Dp _glTexCoord3dv;
        private static DEL_V_FFF _glTexCoord3f;
        private static DEL_V_Fp _glTexCoord3fv;
        private static DEL_V_III _glTexCoord3i;
        private static DEL_V_Ip _glTexCoord3iv;
        private static DEL_V_SSS _glTexCoord3s;
        private static DEL_V_Sp _glTexCoord3sv;
        private static DEL_V_DDDD _glTexCoord4d;
        private static DEL_V_Dp _glTexCoord4dv;
        private static DEL_V_FFFF _glTexCoord4f;
        private static DEL_V_Fp _glTexCoord4fv;
        private static DEL_V_IIII _glTexCoord4i;
        private static DEL_V_Ip _glTexCoord4iv;
        private static DEL_V_SSSS _glTexCoord4s;
        private static DEL_V_Sp _glTexCoord4sv;
        private static DEL_V_ETexCoordPointerTypeEUi _glTexCoordP1ui;
        private static DEL_V_ETexCoordPointerTypeEUip _glTexCoordP1uiv;
        private static DEL_V_ETexCoordPointerTypeEUi _glTexCoordP2ui;
        private static DEL_V_ETexCoordPointerTypeEUip _glTexCoordP2uiv;
        private static DEL_V_ETexCoordPointerTypeEUi _glTexCoordP3ui;
        private static DEL_V_ETexCoordPointerTypeEUip _glTexCoordP3uiv;
        private static DEL_V_ETexCoordPointerTypeEUi _glTexCoordP4ui;
        private static DEL_V_ETexCoordPointerTypeEUip _glTexCoordP4uiv;
        private static DEL_V_IETexCoordPointerTypeEIVp _glTexCoordPointer;
        private static DEL_V_ETextureEnvTargetEETextureEnvParameterEF _glTexEnvf;
        private static DEL_V_ETextureEnvTargetEETextureEnvParameterEFp _glTexEnvfv;
        private static DEL_V_ETextureEnvTargetEETextureEnvParameterEI _glTexEnvi;
        private static DEL_V_ETextureEnvTargetEETextureEnvParameterEIp _glTexEnviv;
        private static DEL_V_ETextureCoordNameEETextureGenParameterED _glTexGend;
        private static DEL_V_ETextureCoordNameEETextureGenParameterEDp _glTexGendv;
        private static DEL_V_ETextureCoordNameEETextureGenParameterEF _glTexGenf;
        private static DEL_V_ETextureCoordNameEETextureGenParameterEFp _glTexGenfv;
        private static DEL_V_ETextureCoordNameEETextureGenParameterEI _glTexGeni;
        private static DEL_V_ETextureCoordNameEETextureGenParameterEIp _glTexGeniv;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIEPixelFormatEEPixelTypeEVp _glTexImage1D;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIIEPixelFormatEEPixelTypeEVp _glTexImage2D;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIB _glTexImage2DMultisample;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIIIEPixelFormatEEPixelTypeEVp _glTexImage3D;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIIB _glTexImage3DMultisample;
        private static DEL_V_ETextureTargetEETextureParameterNameEF _glTexParameterf;
        private static DEL_V_ETextureTargetEETextureParameterNameEFp _glTexParameterfv;
        private static DEL_V_ETextureTargetEETextureParameterNameEI _glTexParameteri;
        private static DEL_V_ETextureTargetEETextureParameterNameEIp _glTexParameterIiv;
        private static DEL_V_ETextureTargetEETextureParameterNameEUip _glTexParameterIuiv;
        private static DEL_V_ETextureTargetEETextureParameterNameEIp _glTexParameteriv;
        private static DEL_V_ETextureTargetEIEInternalFormatEI _glTexStorage1D;
        private static DEL_V_ETextureTargetEIEInternalFormatEII _glTexStorage2D;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIB _glTexStorage2DMultisample;
        private static DEL_V_ETextureTargetEIEInternalFormatEIII _glTexStorage3D;
        private static DEL_V_ETextureTargetEIEInternalFormatEIIIB _glTexStorage3DMultisample;
        private static DEL_V_ETextureTargetEIIIEPixelFormatEEPixelTypeEVp _glTexSubImage1D;
        private static DEL_V_ETextureTargetEIIIIIEPixelFormatEEPixelTypeEVp _glTexSubImage2D;
        private static DEL_V_ETextureTargetEIIIIIIIEPixelFormatEEPixelTypeEVp _glTexSubImage3D;
        private static DEL_V_ _glTextureBarrier;
        private static DEL_V_UiEInternalFormatEUi _glTextureBuffer;
        private static DEL_V_UiEInternalFormatEUiPP _glTextureBufferRange;
        private static DEL_V_UiETextureParameterNameEF _glTextureParameterf;
        private static DEL_V_UiETextureParameterNameEFp _glTextureParameterfv;
        private static DEL_V_UiETextureParameterNameEI _glTextureParameteri;
        private static DEL_V_UiETextureParameterNameEIp _glTextureParameterIiv;
        private static DEL_V_UiETextureParameterNameEUip _glTextureParameterIuiv;
        private static DEL_V_UiETextureParameterNameEIp _glTextureParameteriv;
        private static DEL_V_UiIEInternalFormatEI _glTextureStorage1D;
        private static DEL_V_UiIEInternalFormatEII _glTextureStorage2D;
        private static DEL_V_UiIEInternalFormatEIIB _glTextureStorage2DMultisample;
        private static DEL_V_UiIEInternalFormatEIII _glTextureStorage3D;
        private static DEL_V_UiIEInternalFormatEIIIB _glTextureStorage3DMultisample;
        private static DEL_V_UiIIIEPixelFormatEEPixelTypeEVp _glTextureSubImage1D;
        private static DEL_V_UiIIIIIEPixelFormatEEPixelTypeEVp _glTextureSubImage2D;
        private static DEL_V_UiIIIIIIIEPixelFormatEEPixelTypeEVp _glTextureSubImage3D;
        private static DEL_V_UiETextureTargetEUiEInternalFormatEUiUiUiUi _glTextureView;
        private static DEL_V_UiUiUi _glTransformFeedbackBufferBase;
        private static DEL_V_UiUiUiPP _glTransformFeedbackBufferRange;
        private static DEL_V_UiICppETransformFeedbackBufferModeE _glTransformFeedbackVaryings;
        private static DEL_V_DDD _glTranslated;
        private static DEL_V_FFF _glTranslatef;
        private static DEL_V_ID _glUniform1d;
        private static DEL_V_IIDp _glUniform1dv;
        private static DEL_V_IF _glUniform1f;
        private static DEL_V_IIFp _glUniform1fv;
        private static DEL_V_II _glUniform1i;
        private static DEL_V_IIIp _glUniform1iv;
        private static DEL_V_IUi _glUniform1ui;
        private static DEL_V_IIUip _glUniform1uiv;
        private static DEL_V_IDD _glUniform2d;
        private static DEL_V_IIDp _glUniform2dv;
        private static DEL_V_IFF _glUniform2f;
        private static DEL_V_IIFp _glUniform2fv;
        private static DEL_V_III _glUniform2i;
        private static DEL_V_IIIp _glUniform2iv;
        private static DEL_V_IUiUi _glUniform2ui;
        private static DEL_V_IIUip _glUniform2uiv;
        private static DEL_V_IDDD _glUniform3d;
        private static DEL_V_IIDp _glUniform3dv;
        private static DEL_V_IFFF _glUniform3f;
        private static DEL_V_IIFp _glUniform3fv;
        private static DEL_V_IIII _glUniform3i;
        private static DEL_V_IIIp _glUniform3iv;
        private static DEL_V_IUiUiUi _glUniform3ui;
        private static DEL_V_IIUip _glUniform3uiv;
        private static DEL_V_IDDDD _glUniform4d;
        private static DEL_V_IIDp _glUniform4dv;
        private static DEL_V_IFFFF _glUniform4f;
        private static DEL_V_IIFp _glUniform4fv;
        private static DEL_V_IIIII _glUniform4i;
        private static DEL_V_IIIp _glUniform4iv;
        private static DEL_V_IUiUiUiUi _glUniform4ui;
        private static DEL_V_IIUip _glUniform4uiv;
        private static DEL_V_UiUiUi _glUniformBlockBinding;
        private static DEL_V_IIBDp _glUniformMatrix2dv;
        private static DEL_V_IIBFp _glUniformMatrix2fv;
        private static DEL_V_IIBDp _glUniformMatrix2x3dv;
        private static DEL_V_IIBFp _glUniformMatrix2x3fv;
        private static DEL_V_IIBDp _glUniformMatrix2x4dv;
        private static DEL_V_IIBFp _glUniformMatrix2x4fv;
        private static DEL_V_IIBDp _glUniformMatrix3dv;
        private static DEL_V_IIBFp _glUniformMatrix3fv;
        private static DEL_V_IIBDp _glUniformMatrix3x2dv;
        private static DEL_V_IIBFp _glUniformMatrix3x2fv;
        private static DEL_V_IIBDp _glUniformMatrix3x4dv;
        private static DEL_V_IIBFp _glUniformMatrix3x4fv;
        private static DEL_V_IIBDp _glUniformMatrix4dv;
        private static DEL_V_IIBFp _glUniformMatrix4fv;
        private static DEL_V_IIBDp _glUniformMatrix4x2dv;
        private static DEL_V_IIBFp _glUniformMatrix4x2fv;
        private static DEL_V_IIBDp _glUniformMatrix4x3dv;
        private static DEL_V_IIBFp _glUniformMatrix4x3fv;
        private static DEL_V_EShaderTypeEIUip _glUniformSubroutinesuiv;
        private static DEL_B_EBufferTargetARBE _glUnmapBuffer;
        private static DEL_B_Ui _glUnmapNamedBuffer;
        private static DEL_V_Ui _glUseProgram;
        private static DEL_V_UiEUseProgramStageMaskEUi _glUseProgramStages;
        private static DEL_V_Ui _glValidateProgram;
        private static DEL_V_Ui _glValidateProgramPipeline;
        private static DEL_V_DD _glVertex2d;
        private static DEL_V_Dp _glVertex2dv;
        private static DEL_V_FF _glVertex2f;
        private static DEL_V_Fp _glVertex2fv;
        private static DEL_V_II _glVertex2i;
        private static DEL_V_Ip _glVertex2iv;
        private static DEL_V_SS _glVertex2s;
        private static DEL_V_Sp _glVertex2sv;
        private static DEL_V_DDD _glVertex3d;
        private static DEL_V_Dp _glVertex3dv;
        private static DEL_V_FFF _glVertex3f;
        private static DEL_V_Fp _glVertex3fv;
        private static DEL_V_III _glVertex3i;
        private static DEL_V_Ip _glVertex3iv;
        private static DEL_V_SSS _glVertex3s;
        private static DEL_V_Sp _glVertex3sv;
        private static DEL_V_DDDD _glVertex4d;
        private static DEL_V_Dp _glVertex4dv;
        private static DEL_V_FFFF _glVertex4f;
        private static DEL_V_Fp _glVertex4fv;
        private static DEL_V_IIII _glVertex4i;
        private static DEL_V_Ip _glVertex4iv;
        private static DEL_V_SSSS _glVertex4s;
        private static DEL_V_Sp _glVertex4sv;
        private static DEL_V_UiUiUi _glVertexArrayAttribBinding;
        private static DEL_V_UiUiIEVertexAttribTypeEBUi _glVertexArrayAttribFormat;
        private static DEL_V_UiUiIEVertexAttribITypeEUi _glVertexArrayAttribIFormat;
        private static DEL_V_UiUiIEVertexAttribLTypeEUi _glVertexArrayAttribLFormat;
        private static DEL_V_UiUiUi _glVertexArrayBindingDivisor;
        private static DEL_V_UiUi _glVertexArrayElementBuffer;
        private static DEL_V_UiUiUiPI _glVertexArrayVertexBuffer;
        private static DEL_V_UiUiIUipPpIp _glVertexArrayVertexBuffers;
        private static DEL_V_UiD _glVertexAttrib1d;
        private static DEL_V_UiDp _glVertexAttrib1dv;
        private static DEL_V_UiF _glVertexAttrib1f;
        private static DEL_V_UiFp _glVertexAttrib1fv;
        private static DEL_V_UiS _glVertexAttrib1s;
        private static DEL_V_UiSp _glVertexAttrib1sv;
        private static DEL_V_UiDD _glVertexAttrib2d;
        private static DEL_V_UiDp _glVertexAttrib2dv;
        private static DEL_V_UiFF _glVertexAttrib2f;
        private static DEL_V_UiFp _glVertexAttrib2fv;
        private static DEL_V_UiSS _glVertexAttrib2s;
        private static DEL_V_UiSp _glVertexAttrib2sv;
        private static DEL_V_UiDDD _glVertexAttrib3d;
        private static DEL_V_UiDp _glVertexAttrib3dv;
        private static DEL_V_UiFFF _glVertexAttrib3f;
        private static DEL_V_UiFp _glVertexAttrib3fv;
        private static DEL_V_UiSSS _glVertexAttrib3s;
        private static DEL_V_UiSp _glVertexAttrib3sv;
        private static DEL_V_UiByp _glVertexAttrib4bv;
        private static DEL_V_UiDDDD _glVertexAttrib4d;
        private static DEL_V_UiDp _glVertexAttrib4dv;
        private static DEL_V_UiFFFF _glVertexAttrib4f;
        private static DEL_V_UiFp _glVertexAttrib4fv;
        private static DEL_V_UiIp _glVertexAttrib4iv;
        private static DEL_V_UiByp _glVertexAttrib4Nbv;
        private static DEL_V_UiIp _glVertexAttrib4Niv;
        private static DEL_V_UiSp _glVertexAttrib4Nsv;
        private static DEL_V_UiByByByBy _glVertexAttrib4Nub;
        private static DEL_V_UiByp _glVertexAttrib4Nubv;
        private static DEL_V_UiUip _glVertexAttrib4Nuiv;
        private static DEL_V_UiUsp _glVertexAttrib4Nusv;
        private static DEL_V_UiSSSS _glVertexAttrib4s;
        private static DEL_V_UiSp _glVertexAttrib4sv;
        private static DEL_V_UiByp _glVertexAttrib4ubv;
        private static DEL_V_UiUip _glVertexAttrib4uiv;
        private static DEL_V_UiUsp _glVertexAttrib4usv;
        private static DEL_V_UiUi _glVertexAttribBinding;
        private static DEL_V_UiUi _glVertexAttribDivisor;
        private static DEL_V_UiIEVertexAttribTypeEBUi _glVertexAttribFormat;
        private static DEL_V_UiI _glVertexAttribI1i;
        private static DEL_V_UiIp _glVertexAttribI1iv;
        private static DEL_V_UiUi _glVertexAttribI1ui;
        private static DEL_V_UiUip _glVertexAttribI1uiv;
        private static DEL_V_UiII _glVertexAttribI2i;
        private static DEL_V_UiIp _glVertexAttribI2iv;
        private static DEL_V_UiUiUi _glVertexAttribI2ui;
        private static DEL_V_UiUip _glVertexAttribI2uiv;
        private static DEL_V_UiIII _glVertexAttribI3i;
        private static DEL_V_UiIp _glVertexAttribI3iv;
        private static DEL_V_UiUiUiUi _glVertexAttribI3ui;
        private static DEL_V_UiUip _glVertexAttribI3uiv;
        private static DEL_V_UiByp _glVertexAttribI4bv;
        private static DEL_V_UiIIII _glVertexAttribI4i;
        private static DEL_V_UiIp _glVertexAttribI4iv;
        private static DEL_V_UiSp _glVertexAttribI4sv;
        private static DEL_V_UiByp _glVertexAttribI4ubv;
        private static DEL_V_UiUiUiUiUi _glVertexAttribI4ui;
        private static DEL_V_UiUip _glVertexAttribI4uiv;
        private static DEL_V_UiUsp _glVertexAttribI4usv;
        private static DEL_V_UiIEVertexAttribITypeEUi _glVertexAttribIFormat;
        private static DEL_V_UiIEVertexAttribITypeEIVp _glVertexAttribIPointer;
        private static DEL_V_UiD _glVertexAttribL1d;
        private static DEL_V_UiDp _glVertexAttribL1dv;
        private static DEL_V_UiDD _glVertexAttribL2d;
        private static DEL_V_UiDp _glVertexAttribL2dv;
        private static DEL_V_UiDDD _glVertexAttribL3d;
        private static DEL_V_UiDp _glVertexAttribL3dv;
        private static DEL_V_UiDDDD _glVertexAttribL4d;
        private static DEL_V_UiDp _glVertexAttribL4dv;
        private static DEL_V_UiIEVertexAttribLTypeEUi _glVertexAttribLFormat;
        private static DEL_V_UiIEVertexAttribLTypeEIVp _glVertexAttribLPointer;
        private static DEL_V_UiEVertexAttribPointerTypeEBUi _glVertexAttribP1ui;
        private static DEL_V_UiEVertexAttribPointerTypeEBUip _glVertexAttribP1uiv;
        private static DEL_V_UiEVertexAttribPointerTypeEBUi _glVertexAttribP2ui;
        private static DEL_V_UiEVertexAttribPointerTypeEBUip _glVertexAttribP2uiv;
        private static DEL_V_UiEVertexAttribPointerTypeEBUi _glVertexAttribP3ui;
        private static DEL_V_UiEVertexAttribPointerTypeEBUip _glVertexAttribP3uiv;
        private static DEL_V_UiEVertexAttribPointerTypeEBUi _glVertexAttribP4ui;
        private static DEL_V_UiEVertexAttribPointerTypeEBUip _glVertexAttribP4uiv;
        private static DEL_V_UiIEVertexAttribPointerTypeEBIVp _glVertexAttribPointer;
        private static DEL_V_UiUi _glVertexBindingDivisor;
        private static DEL_V_EVertexPointerTypeEUi _glVertexP2ui;
        private static DEL_V_EVertexPointerTypeEUip _glVertexP2uiv;
        private static DEL_V_EVertexPointerTypeEUi _glVertexP3ui;
        private static DEL_V_EVertexPointerTypeEUip _glVertexP3uiv;
        private static DEL_V_EVertexPointerTypeEUi _glVertexP4ui;
        private static DEL_V_EVertexPointerTypeEUip _glVertexP4uiv;
        private static DEL_V_IEVertexPointerTypeEIVp _glVertexPointer;
        private static DEL_V_IIII _glViewport;
        private static DEL_V_UiIFp _glViewportArrayv;
        private static DEL_V_UiFFFF _glViewportIndexedf;
        private static DEL_V_UiFp _glViewportIndexedfv;
        private static DEL_V_StpSyncStpESyncBehaviorFlagsEUl _glWaitSync;
        private static DEL_V_DD _glWindowPos2d;
        private static DEL_V_Dp _glWindowPos2dv;
        private static DEL_V_FF _glWindowPos2f;
        private static DEL_V_Fp _glWindowPos2fv;
        private static DEL_V_II _glWindowPos2i;
        private static DEL_V_Ip _glWindowPos2iv;
        private static DEL_V_SS _glWindowPos2s;
        private static DEL_V_Sp _glWindowPos2sv;
        private static DEL_V_DDD _glWindowPos3d;
        private static DEL_V_Dp _glWindowPos3dv;
        private static DEL_V_FFF _glWindowPos3f;
        private static DEL_V_Fp _glWindowPos3fv;
        private static DEL_V_III _glWindowPos3i;
        private static DEL_V_Ip _glWindowPos3iv;
        private static DEL_V_SSS _glWindowPos3s;
        private static DEL_V_Sp _glWindowPos3sv;

        public static void LoadFunctions()
        {
            _glAccum = GetFunctionDelegate<DEL_V_EAccumOpEF>("glAccum");
            _glActiveShaderProgram = GetFunctionDelegate<DEL_V_UiUi>("glActiveShaderProgram");
            _glActiveTexture = GetFunctionDelegate<DEL_V_ETextureUnitE>("glActiveTexture");
            _glAlphaFunc = GetFunctionDelegate<DEL_V_EAlphaFunctionEF>("glAlphaFunc");
            _glAreTexturesResident = GetFunctionDelegate<DEL_B_IUipBp>("glAreTexturesResident");
            _glArrayElement = GetFunctionDelegate<DEL_V_I>("glArrayElement");
            _glAttachShader = GetFunctionDelegate<DEL_V_UiUi>("glAttachShader");
            _glBegin = GetFunctionDelegate<DEL_V_EPrimitiveTypeE>("glBegin");
            _glBeginConditionalRender = GetFunctionDelegate<DEL_V_UiEConditionalRenderModeE>("glBeginConditionalRender");
            _glBeginQuery = GetFunctionDelegate<DEL_V_EQueryTargetEUi>("glBeginQuery");
            _glBeginQueryIndexed = GetFunctionDelegate<DEL_V_EQueryTargetEUiUi>("glBeginQueryIndexed");
            _glBeginTransformFeedback = GetFunctionDelegate<DEL_V_EPrimitiveTypeE>("glBeginTransformFeedback");
            _glBindAttribLocation = GetFunctionDelegate<DEL_V_UiUiCp>("glBindAttribLocation");
            _glBindBuffer = GetFunctionDelegate<DEL_V_EBufferTargetARBEUi>("glBindBuffer");
            _glBindBufferBase = GetFunctionDelegate<DEL_V_EBufferTargetARBEUiUi>("glBindBufferBase");
            _glBindBufferRange = GetFunctionDelegate<DEL_V_EBufferTargetARBEUiUiPP>("glBindBufferRange");
            _glBindBuffersBase = GetFunctionDelegate<DEL_V_EBufferTargetARBEUiIUip>("glBindBuffersBase");
            _glBindBuffersRange = GetFunctionDelegate<DEL_V_EBufferTargetARBEUiIUipPpPp>("glBindBuffersRange");
            _glBindFragDataLocation = GetFunctionDelegate<DEL_V_UiUiCp>("glBindFragDataLocation");
            _glBindFragDataLocationIndexed = GetFunctionDelegate<DEL_V_UiUiUiCp>("glBindFragDataLocationIndexed");
            _glBindFramebuffer = GetFunctionDelegate<DEL_V_EFramebufferTargetEUi>("glBindFramebuffer");
            _glBindImageTexture = GetFunctionDelegate<DEL_V_UiUiIBIEBufferAccessARBEEInternalFormatE>("glBindImageTexture");
            _glBindImageTextures = GetFunctionDelegate<DEL_V_UiIUip>("glBindImageTextures");
            _glBindProgramPipeline = GetFunctionDelegate<DEL_V_Ui>("glBindProgramPipeline");
            _glBindRenderbuffer = GetFunctionDelegate<DEL_V_ERenderbufferTargetEUi>("glBindRenderbuffer");
            _glBindSampler = GetFunctionDelegate<DEL_V_UiUi>("glBindSampler");
            _glBindSamplers = GetFunctionDelegate<DEL_V_UiIUip>("glBindSamplers");
            _glBindTexture = GetFunctionDelegate<DEL_V_ETextureTargetEUi>("glBindTexture");
            _glBindTextures = GetFunctionDelegate<DEL_V_UiIUip>("glBindTextures");
            _glBindTextureUnit = GetFunctionDelegate<DEL_V_UiUi>("glBindTextureUnit");
            _glBindTransformFeedback = GetFunctionDelegate<DEL_V_EBindTransformFeedbackTargetEUi>("glBindTransformFeedback");
            _glBindVertexArray = GetFunctionDelegate<DEL_V_Ui>("glBindVertexArray");
            _glBindVertexBuffer = GetFunctionDelegate<DEL_V_UiUiPI>("glBindVertexBuffer");
            _glBindVertexBuffers = GetFunctionDelegate<DEL_V_UiIUipPpIp>("glBindVertexBuffers");
            _glBitmap = GetFunctionDelegate<DEL_V_IIFFFFByp>("glBitmap");
            _glBlendColor = GetFunctionDelegate<DEL_V_FFFF>("glBlendColor");
            _glBlendEquation = GetFunctionDelegate<DEL_V_EBlendEquationModeEXTE>("glBlendEquation");
            _glBlendEquationi = GetFunctionDelegate<DEL_V_UiEBlendEquationModeEXTE>("glBlendEquationi");
            _glBlendEquationSeparate = GetFunctionDelegate<DEL_V_EBlendEquationModeEXTEEBlendEquationModeEXTE>("glBlendEquationSeparate");
            _glBlendEquationSeparatei = GetFunctionDelegate<DEL_V_UiEBlendEquationModeEXTEEBlendEquationModeEXTE>("glBlendEquationSeparatei");
            _glBlendFunc = GetFunctionDelegate<DEL_V_EBlendingFactorEEBlendingFactorE>("glBlendFunc");
            _glBlendFunci = GetFunctionDelegate<DEL_V_UiEBlendingFactorEEBlendingFactorE>("glBlendFunci");
            _glBlendFuncSeparate = GetFunctionDelegate<DEL_V_EBlendingFactorEEBlendingFactorEEBlendingFactorEEBlendingFactorE>("glBlendFuncSeparate");
            _glBlendFuncSeparatei = GetFunctionDelegate<DEL_V_UiEBlendingFactorEEBlendingFactorEEBlendingFactorEEBlendingFactorE>("glBlendFuncSeparatei");
            _glBlitFramebuffer = GetFunctionDelegate<DEL_V_IIIIIIIIEClearBufferMaskEEBlitFramebufferFilterE>("glBlitFramebuffer");
            _glBlitNamedFramebuffer = GetFunctionDelegate<DEL_V_UiUiIIIIIIIIEClearBufferMaskEEBlitFramebufferFilterE>("glBlitNamedFramebuffer");
            _glBufferData = GetFunctionDelegate<DEL_V_EBufferTargetARBEPVpEBufferUsageARBE>("glBufferData");
            _glBufferStorage = GetFunctionDelegate<DEL_V_EBufferStorageTargetEPVpEBufferStorageMaskE>("glBufferStorage");
            _glBufferSubData = GetFunctionDelegate<DEL_V_EBufferTargetARBEPPVp>("glBufferSubData");
            _glCallList = GetFunctionDelegate<DEL_V_Ui>("glCallList");
            _glCallLists = GetFunctionDelegate<DEL_V_IEListNameTypeEVp>("glCallLists");
            _glCheckFramebufferStatus = GetFunctionDelegate<DEL_EFramebufferStatusE_EFramebufferTargetE>("glCheckFramebufferStatus");
            _glCheckNamedFramebufferStatus = GetFunctionDelegate<DEL_EFramebufferStatusE_UiEFramebufferTargetE>("glCheckNamedFramebufferStatus");
            _glClampColor = GetFunctionDelegate<DEL_V_EClampColorTargetARBEEClampColorModeARBE>("glClampColor");
            _glClear = GetFunctionDelegate<DEL_V_EClearBufferMaskE>("glClear");
            _glClearAccum = GetFunctionDelegate<DEL_V_FFFF>("glClearAccum");
            _glClearBufferData = GetFunctionDelegate<DEL_V_EBufferStorageTargetEEInternalFormatEEPixelFormatEEPixelTypeEVp>("glClearBufferData");
            _glClearBufferfi = GetFunctionDelegate<DEL_V_EBufferEIFI>("glClearBufferfi");
            _glClearBufferfv = GetFunctionDelegate<DEL_V_EBufferEIFp>("glClearBufferfv");
            _glClearBufferiv = GetFunctionDelegate<DEL_V_EBufferEIIp>("glClearBufferiv");
            _glClearBufferSubData = GetFunctionDelegate<DEL_V_EBufferTargetARBEEInternalFormatEPPEPixelFormatEEPixelTypeEVp>("glClearBufferSubData");
            _glClearBufferuiv = GetFunctionDelegate<DEL_V_EBufferEIUip>("glClearBufferuiv");
            _glClearColor = GetFunctionDelegate<DEL_V_FFFF>("glClearColor");
            _glClearDepth = GetFunctionDelegate<DEL_V_D>("glClearDepth");
            _glClearDepthf = GetFunctionDelegate<DEL_V_F>("glClearDepthf");
            _glClearIndex = GetFunctionDelegate<DEL_V_F>("glClearIndex");
            _glClearNamedBufferData = GetFunctionDelegate<DEL_V_UiEInternalFormatEEPixelFormatEEPixelTypeEVp>("glClearNamedBufferData");
            _glClearNamedBufferSubData = GetFunctionDelegate<DEL_V_UiEInternalFormatEPPEPixelFormatEEPixelTypeEVp>("glClearNamedBufferSubData");
            _glClearNamedFramebufferfi = GetFunctionDelegate<DEL_V_UiEBufferEIFI>("glClearNamedFramebufferfi");
            _glClearNamedFramebufferfv = GetFunctionDelegate<DEL_V_UiEBufferEIFp>("glClearNamedFramebufferfv");
            _glClearNamedFramebufferiv = GetFunctionDelegate<DEL_V_UiEBufferEIIp>("glClearNamedFramebufferiv");
            _glClearNamedFramebufferuiv = GetFunctionDelegate<DEL_V_UiEBufferEIUip>("glClearNamedFramebufferuiv");
            _glClearStencil = GetFunctionDelegate<DEL_V_I>("glClearStencil");
            _glClearTexImage = GetFunctionDelegate<DEL_V_UiIEPixelFormatEEPixelTypeEVp>("glClearTexImage");
            _glClearTexSubImage = GetFunctionDelegate<DEL_V_UiIIIIIIIEPixelFormatEEPixelTypeEVp>("glClearTexSubImage");
            _glClientActiveTexture = GetFunctionDelegate<DEL_V_ETextureUnitE>("glClientActiveTexture");
            _glClientWaitSync = GetFunctionDelegate<DEL_ESyncStatusE_StpSyncStpESyncObjectMaskEUl>("glClientWaitSync");
            _glClipControl = GetFunctionDelegate<DEL_V_EClipControlOriginEEClipControlDepthE>("glClipControl");
            _glClipPlane = GetFunctionDelegate<DEL_V_EClipPlaneNameEDp>("glClipPlane");
            _glColor3b = GetFunctionDelegate<DEL_V_ByByBy>("glColor3b");
            _glColor3bv = GetFunctionDelegate<DEL_V_Byp>("glColor3bv");
            _glColor3d = GetFunctionDelegate<DEL_V_DDD>("glColor3d");
            _glColor3dv = GetFunctionDelegate<DEL_V_Dp>("glColor3dv");
            _glColor3f = GetFunctionDelegate<DEL_V_FFF>("glColor3f");
            _glColor3fv = GetFunctionDelegate<DEL_V_Fp>("glColor3fv");
            _glColor3i = GetFunctionDelegate<DEL_V_III>("glColor3i");
            _glColor3iv = GetFunctionDelegate<DEL_V_Ip>("glColor3iv");
            _glColor3s = GetFunctionDelegate<DEL_V_SSS>("glColor3s");
            _glColor3sv = GetFunctionDelegate<DEL_V_Sp>("glColor3sv");
            _glColor3ub = GetFunctionDelegate<DEL_V_ByByBy>("glColor3ub");
            _glColor3ubv = GetFunctionDelegate<DEL_V_Byp>("glColor3ubv");
            _glColor3ui = GetFunctionDelegate<DEL_V_UiUiUi>("glColor3ui");
            _glColor3uiv = GetFunctionDelegate<DEL_V_Uip>("glColor3uiv");
            _glColor3us = GetFunctionDelegate<DEL_V_UsUsUs>("glColor3us");
            _glColor3usv = GetFunctionDelegate<DEL_V_Usp>("glColor3usv");
            _glColor4b = GetFunctionDelegate<DEL_V_ByByByBy>("glColor4b");
            _glColor4bv = GetFunctionDelegate<DEL_V_Byp>("glColor4bv");
            _glColor4d = GetFunctionDelegate<DEL_V_DDDD>("glColor4d");
            _glColor4dv = GetFunctionDelegate<DEL_V_Dp>("glColor4dv");
            _glColor4f = GetFunctionDelegate<DEL_V_FFFF>("glColor4f");
            _glColor4fv = GetFunctionDelegate<DEL_V_Fp>("glColor4fv");
            _glColor4i = GetFunctionDelegate<DEL_V_IIII>("glColor4i");
            _glColor4iv = GetFunctionDelegate<DEL_V_Ip>("glColor4iv");
            _glColor4s = GetFunctionDelegate<DEL_V_SSSS>("glColor4s");
            _glColor4sv = GetFunctionDelegate<DEL_V_Sp>("glColor4sv");
            _glColor4ub = GetFunctionDelegate<DEL_V_ByByByBy>("glColor4ub");
            _glColor4ubv = GetFunctionDelegate<DEL_V_Byp>("glColor4ubv");
            _glColor4ui = GetFunctionDelegate<DEL_V_UiUiUiUi>("glColor4ui");
            _glColor4uiv = GetFunctionDelegate<DEL_V_Uip>("glColor4uiv");
            _glColor4us = GetFunctionDelegate<DEL_V_UsUsUsUs>("glColor4us");
            _glColor4usv = GetFunctionDelegate<DEL_V_Usp>("glColor4usv");
            _glColorMask = GetFunctionDelegate<DEL_V_BBBB>("glColorMask");
            _glColorMaski = GetFunctionDelegate<DEL_V_UiBBBB>("glColorMaski");
            _glColorMaterial = GetFunctionDelegate<DEL_V_EMaterialFaceEEColorMaterialParameterE>("glColorMaterial");
            _glColorP3ui = GetFunctionDelegate<DEL_V_EColorPointerTypeEUi>("glColorP3ui");
            _glColorP3uiv = GetFunctionDelegate<DEL_V_EColorPointerTypeEUip>("glColorP3uiv");
            _glColorP4ui = GetFunctionDelegate<DEL_V_EColorPointerTypeEUi>("glColorP4ui");
            _glColorP4uiv = GetFunctionDelegate<DEL_V_EColorPointerTypeEUip>("glColorP4uiv");
            _glColorPointer = GetFunctionDelegate<DEL_V_IEColorPointerTypeEIVp>("glColorPointer");
            _glCompileShader = GetFunctionDelegate<DEL_V_Ui>("glCompileShader");
            _glCompressedTexImage1D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIIVp>("glCompressedTexImage1D");
            _glCompressedTexImage2D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIIIVp>("glCompressedTexImage2D");
            _glCompressedTexImage3D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIIIIVp>("glCompressedTexImage3D");
            _glCompressedTexSubImage1D = GetFunctionDelegate<DEL_V_ETextureTargetEIIIEPixelFormatEIVp>("glCompressedTexSubImage1D");
            _glCompressedTexSubImage2D = GetFunctionDelegate<DEL_V_ETextureTargetEIIIIIEPixelFormatEIVp>("glCompressedTexSubImage2D");
            _glCompressedTexSubImage3D = GetFunctionDelegate<DEL_V_ETextureTargetEIIIIIIIEPixelFormatEIVp>("glCompressedTexSubImage3D");
            _glCompressedTextureSubImage1D = GetFunctionDelegate<DEL_V_UiIIIEPixelFormatEIVp>("glCompressedTextureSubImage1D");
            _glCompressedTextureSubImage2D = GetFunctionDelegate<DEL_V_UiIIIIIEPixelFormatEIVp>("glCompressedTextureSubImage2D");
            _glCompressedTextureSubImage3D = GetFunctionDelegate<DEL_V_UiIIIIIIIEPixelFormatEIVp>("glCompressedTextureSubImage3D");
            _glCopyBufferSubData = GetFunctionDelegate<DEL_V_ECopyBufferSubDataTargetEECopyBufferSubDataTargetEPPP>("glCopyBufferSubData");
            _glCopyImageSubData = GetFunctionDelegate<DEL_V_UiECopyImageSubDataTargetEIIIIUiECopyImageSubDataTargetEIIIIIII>("glCopyImageSubData");
            _glCopyNamedBufferSubData = GetFunctionDelegate<DEL_V_UiUiPPP>("glCopyNamedBufferSubData");
            _glCopyPixels = GetFunctionDelegate<DEL_V_IIIIEPixelCopyTypeE>("glCopyPixels");
            _glCopyTexImage1D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIII>("glCopyTexImage1D");
            _glCopyTexImage2D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIIII>("glCopyTexImage2D");
            _glCopyTexSubImage1D = GetFunctionDelegate<DEL_V_ETextureTargetEIIIII>("glCopyTexSubImage1D");
            _glCopyTexSubImage2D = GetFunctionDelegate<DEL_V_ETextureTargetEIIIIIII>("glCopyTexSubImage2D");
            _glCopyTexSubImage3D = GetFunctionDelegate<DEL_V_ETextureTargetEIIIIIIII>("glCopyTexSubImage3D");
            _glCopyTextureSubImage1D = GetFunctionDelegate<DEL_V_UiIIIII>("glCopyTextureSubImage1D");
            _glCopyTextureSubImage2D = GetFunctionDelegate<DEL_V_UiIIIIIII>("glCopyTextureSubImage2D");
            _glCopyTextureSubImage3D = GetFunctionDelegate<DEL_V_UiIIIIIIII>("glCopyTextureSubImage3D");
            _glCreateBuffers = GetFunctionDelegate<DEL_V_IUip>("glCreateBuffers");
            _glCreateFramebuffers = GetFunctionDelegate<DEL_V_IUip>("glCreateFramebuffers");
            _glCreateProgram = GetFunctionDelegate<DEL_Ui_>("glCreateProgram");
            _glCreateProgramPipelines = GetFunctionDelegate<DEL_V_IUip>("glCreateProgramPipelines");
            _glCreateQueries = GetFunctionDelegate<DEL_V_EQueryTargetEIUip>("glCreateQueries");
            _glCreateRenderbuffers = GetFunctionDelegate<DEL_V_IUip>("glCreateRenderbuffers");
            _glCreateSamplers = GetFunctionDelegate<DEL_V_IUip>("glCreateSamplers");
            _glCreateShader = GetFunctionDelegate<DEL_Ui_EShaderTypeE>("glCreateShader");
            _glCreateShaderProgramv = GetFunctionDelegate<DEL_Ui_EShaderTypeEICpp>("glCreateShaderProgramv");
            _glCreateTextures = GetFunctionDelegate<DEL_V_ETextureTargetEIUip>("glCreateTextures");
            _glCreateTransformFeedbacks = GetFunctionDelegate<DEL_V_IUip>("glCreateTransformFeedbacks");
            _glCreateVertexArrays = GetFunctionDelegate<DEL_V_IUip>("glCreateVertexArrays");
            _glCullFace = GetFunctionDelegate<DEL_V_ECullFaceModeE>("glCullFace");
            _glDebugMessageCallback = GetFunctionDelegate<DEL_V_VpVp>("glDebugMessageCallback");
            _glDebugMessageControl = GetFunctionDelegate<DEL_V_EDebugSourceEEDebugTypeEEDebugSeverityEIUipB>("glDebugMessageControl");
            _glDebugMessageInsert = GetFunctionDelegate<DEL_V_EDebugSourceEEDebugTypeEUiEDebugSeverityEICp>("glDebugMessageInsert");
            _glDeleteBuffers = GetFunctionDelegate<DEL_V_IUip>("glDeleteBuffers");
            _glDeleteFramebuffers = GetFunctionDelegate<DEL_V_IUip>("glDeleteFramebuffers");
            _glDeleteLists = GetFunctionDelegate<DEL_V_UiI>("glDeleteLists");
            _glDeleteProgram = GetFunctionDelegate<DEL_V_Ui>("glDeleteProgram");
            _glDeleteProgramPipelines = GetFunctionDelegate<DEL_V_IUip>("glDeleteProgramPipelines");
            _glDeleteQueries = GetFunctionDelegate<DEL_V_IUip>("glDeleteQueries");
            _glDeleteRenderbuffers = GetFunctionDelegate<DEL_V_IUip>("glDeleteRenderbuffers");
            _glDeleteSamplers = GetFunctionDelegate<DEL_V_IUip>("glDeleteSamplers");
            _glDeleteShader = GetFunctionDelegate<DEL_V_Ui>("glDeleteShader");
            _glDeleteSync = GetFunctionDelegate<DEL_V_StpSyncStp>("glDeleteSync");
            _glDeleteTextures = GetFunctionDelegate<DEL_V_IUip>("glDeleteTextures");
            _glDeleteTransformFeedbacks = GetFunctionDelegate<DEL_V_IUip>("glDeleteTransformFeedbacks");
            _glDeleteVertexArrays = GetFunctionDelegate<DEL_V_IUip>("glDeleteVertexArrays");
            _glDepthFunc = GetFunctionDelegate<DEL_V_EDepthFunctionE>("glDepthFunc");
            _glDepthMask = GetFunctionDelegate<DEL_V_B>("glDepthMask");
            _glDepthRange = GetFunctionDelegate<DEL_V_DD>("glDepthRange");
            _glDepthRangeArrayv = GetFunctionDelegate<DEL_V_UiIDp>("glDepthRangeArrayv");
            _glDepthRangef = GetFunctionDelegate<DEL_V_FF>("glDepthRangef");
            _glDepthRangeIndexed = GetFunctionDelegate<DEL_V_UiDD>("glDepthRangeIndexed");
            _glDetachShader = GetFunctionDelegate<DEL_V_UiUi>("glDetachShader");
            _glDisable = GetFunctionDelegate<DEL_V_EEnableCapE>("glDisable");
            _glDisableClientState = GetFunctionDelegate<DEL_V_EEnableCapE>("glDisableClientState");
            _glDisablei = GetFunctionDelegate<DEL_V_EEnableCapEUi>("glDisablei");
            _glDisableVertexArrayAttrib = GetFunctionDelegate<DEL_V_UiUi>("glDisableVertexArrayAttrib");
            _glDisableVertexAttribArray = GetFunctionDelegate<DEL_V_Ui>("glDisableVertexAttribArray");
            _glDispatchCompute = GetFunctionDelegate<DEL_V_UiUiUi>("glDispatchCompute");
            _glDispatchComputeIndirect = GetFunctionDelegate<DEL_V_P>("glDispatchComputeIndirect");
            _glDrawArrays = GetFunctionDelegate<DEL_V_EPrimitiveTypeEII>("glDrawArrays");
            _glDrawArraysIndirect = GetFunctionDelegate<DEL_V_EPrimitiveTypeEVp>("glDrawArraysIndirect");
            _glDrawArraysInstanced = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIII>("glDrawArraysInstanced");
            _glDrawArraysInstancedBaseInstance = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIIIUi>("glDrawArraysInstancedBaseInstance");
            _glDrawBuffer = GetFunctionDelegate<DEL_V_EDrawBufferModeE>("glDrawBuffer");
            _glDrawBuffers = GetFunctionDelegate<DEL_V_IEpDrawBufferModeEp>("glDrawBuffers");
            _glDrawElements = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVp>("glDrawElements");
            _glDrawElementsBaseVertex = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpI>("glDrawElementsBaseVertex");
            _glDrawElementsIndirect = GetFunctionDelegate<DEL_V_EPrimitiveTypeEEDrawElementsTypeEVp>("glDrawElementsIndirect");
            _glDrawElementsInstanced = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpI>("glDrawElementsInstanced");
            _glDrawElementsInstancedBaseInstance = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIEPrimitiveTypeEVpIUi>("glDrawElementsInstancedBaseInstance");
            _glDrawElementsInstancedBaseVertex = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpII>("glDrawElementsInstancedBaseVertex");
            _glDrawElementsInstancedBaseVertexBaseInstance = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpIIUi>("glDrawElementsInstancedBaseVertexBaseInstance");
            _glDrawPixels = GetFunctionDelegate<DEL_V_IIEPixelFormatEEPixelTypeEVp>("glDrawPixels");
            _glDrawRangeElements = GetFunctionDelegate<DEL_V_EPrimitiveTypeEUiUiIEDrawElementsTypeEVp>("glDrawRangeElements");
            _glDrawRangeElementsBaseVertex = GetFunctionDelegate<DEL_V_EPrimitiveTypeEUiUiIEDrawElementsTypeEVpI>("glDrawRangeElementsBaseVertex");
            _glDrawTransformFeedback = GetFunctionDelegate<DEL_V_EPrimitiveTypeEUi>("glDrawTransformFeedback");
            _glDrawTransformFeedbackInstanced = GetFunctionDelegate<DEL_V_EPrimitiveTypeEUiI>("glDrawTransformFeedbackInstanced");
            _glDrawTransformFeedbackStream = GetFunctionDelegate<DEL_V_EPrimitiveTypeEUiUi>("glDrawTransformFeedbackStream");
            _glDrawTransformFeedbackStreamInstanced = GetFunctionDelegate<DEL_V_EPrimitiveTypeEUiUiI>("glDrawTransformFeedbackStreamInstanced");
            _glEdgeFlag = GetFunctionDelegate<DEL_V_B>("glEdgeFlag");
            _glEdgeFlagPointer = GetFunctionDelegate<DEL_V_IVp>("glEdgeFlagPointer");
            _glEdgeFlagv = GetFunctionDelegate<DEL_V_Bp>("glEdgeFlagv");
            _glEnable = GetFunctionDelegate<DEL_V_EEnableCapE>("glEnable");
            _glEnableClientState = GetFunctionDelegate<DEL_V_EEnableCapE>("glEnableClientState");
            _glEnablei = GetFunctionDelegate<DEL_V_EEnableCapEUi>("glEnablei");
            _glEnableVertexArrayAttrib = GetFunctionDelegate<DEL_V_UiUi>("glEnableVertexArrayAttrib");
            _glEnableVertexAttribArray = GetFunctionDelegate<DEL_V_Ui>("glEnableVertexAttribArray");
            _glEnd = GetFunctionDelegate<DEL_V_>("glEnd");
            _glEndConditionalRender = GetFunctionDelegate<DEL_V_>("glEndConditionalRender");
            _glEndList = GetFunctionDelegate<DEL_V_>("glEndList");
            _glEndQuery = GetFunctionDelegate<DEL_V_EQueryTargetE>("glEndQuery");
            _glEndQueryIndexed = GetFunctionDelegate<DEL_V_EQueryTargetEUi>("glEndQueryIndexed");
            _glEndTransformFeedback = GetFunctionDelegate<DEL_V_>("glEndTransformFeedback");
            _glEvalCoord1d = GetFunctionDelegate<DEL_V_D>("glEvalCoord1d");
            _glEvalCoord1dv = GetFunctionDelegate<DEL_V_Dp>("glEvalCoord1dv");
            _glEvalCoord1f = GetFunctionDelegate<DEL_V_F>("glEvalCoord1f");
            _glEvalCoord1fv = GetFunctionDelegate<DEL_V_Fp>("glEvalCoord1fv");
            _glEvalCoord2d = GetFunctionDelegate<DEL_V_DD>("glEvalCoord2d");
            _glEvalCoord2dv = GetFunctionDelegate<DEL_V_Dp>("glEvalCoord2dv");
            _glEvalCoord2f = GetFunctionDelegate<DEL_V_FF>("glEvalCoord2f");
            _glEvalCoord2fv = GetFunctionDelegate<DEL_V_Fp>("glEvalCoord2fv");
            _glEvalMesh1 = GetFunctionDelegate<DEL_V_EMeshMode1EII>("glEvalMesh1");
            _glEvalMesh2 = GetFunctionDelegate<DEL_V_EMeshMode2EIIII>("glEvalMesh2");
            _glEvalPoint1 = GetFunctionDelegate<DEL_V_I>("glEvalPoint1");
            _glEvalPoint2 = GetFunctionDelegate<DEL_V_II>("glEvalPoint2");
            _glFeedbackBuffer = GetFunctionDelegate<DEL_V_IEFeedbackTypeEFp>("glFeedbackBuffer");
            _glFenceSync = GetFunctionDelegate<DEL_StpSyncStp_ESyncConditionEESyncBehaviorFlagsE>("glFenceSync");
            _glFinish = GetFunctionDelegate<DEL_V_>("glFinish");
            _glFlush = GetFunctionDelegate<DEL_V_>("glFlush");
            _glFlushMappedBufferRange = GetFunctionDelegate<DEL_V_EBufferTargetARBEPP>("glFlushMappedBufferRange");
            _glFlushMappedNamedBufferRange = GetFunctionDelegate<DEL_V_UiPP>("glFlushMappedNamedBufferRange");
            _glFogCoordd = GetFunctionDelegate<DEL_V_D>("glFogCoordd");
            _glFogCoorddv = GetFunctionDelegate<DEL_V_Dp>("glFogCoorddv");
            _glFogCoordf = GetFunctionDelegate<DEL_V_F>("glFogCoordf");
            _glFogCoordfv = GetFunctionDelegate<DEL_V_Fp>("glFogCoordfv");
            _glFogCoordPointer = GetFunctionDelegate<DEL_V_EFogPointerTypeEXTEIVp>("glFogCoordPointer");
            _glFogf = GetFunctionDelegate<DEL_V_EFogParameterEF>("glFogf");
            _glFogfv = GetFunctionDelegate<DEL_V_EFogParameterEFp>("glFogfv");
            _glFogi = GetFunctionDelegate<DEL_V_EFogParameterEI>("glFogi");
            _glFogiv = GetFunctionDelegate<DEL_V_EFogParameterEIp>("glFogiv");
            _glFramebufferParameteri = GetFunctionDelegate<DEL_V_EFramebufferTargetEEFramebufferParameterNameEI>("glFramebufferParameteri");
            _glFramebufferRenderbuffer = GetFunctionDelegate<DEL_V_EFramebufferTargetEEFramebufferAttachmentEERenderbufferTargetEUi>("glFramebufferRenderbuffer");
            _glFramebufferTexture = GetFunctionDelegate<DEL_V_EFramebufferTargetEEFramebufferAttachmentEUiI>("glFramebufferTexture");
            _glFramebufferTexture1D = GetFunctionDelegate<DEL_V_EFramebufferTargetEEFramebufferAttachmentEETextureTargetEUiI>("glFramebufferTexture1D");
            _glFramebufferTexture2D = GetFunctionDelegate<DEL_V_EFramebufferTargetEEFramebufferAttachmentEETextureTargetEUiI>("glFramebufferTexture2D");
            _glFramebufferTexture3D = GetFunctionDelegate<DEL_V_EFramebufferTargetEEFramebufferAttachmentEETextureTargetEUiII>("glFramebufferTexture3D");
            _glFramebufferTextureLayer = GetFunctionDelegate<DEL_V_EFramebufferTargetEEFramebufferAttachmentEUiII>("glFramebufferTextureLayer");
            _glFrontFace = GetFunctionDelegate<DEL_V_EFrontFaceDirectionE>("glFrontFace");
            _glFrustum = GetFunctionDelegate<DEL_V_DDDDDD>("glFrustum");
            _glGenBuffers = GetFunctionDelegate<DEL_V_IUip>("glGenBuffers");
            _glGenerateMipmap = GetFunctionDelegate<DEL_V_ETextureTargetE>("glGenerateMipmap");
            _glGenerateTextureMipmap = GetFunctionDelegate<DEL_V_Ui>("glGenerateTextureMipmap");
            _glGenFramebuffers = GetFunctionDelegate<DEL_V_IUip>("glGenFramebuffers");
            _glGenLists = GetFunctionDelegate<DEL_Ui_I>("glGenLists");
            _glGenProgramPipelines = GetFunctionDelegate<DEL_V_IUip>("glGenProgramPipelines");
            _glGenQueries = GetFunctionDelegate<DEL_V_IUip>("glGenQueries");
            _glGenRenderbuffers = GetFunctionDelegate<DEL_V_IUip>("glGenRenderbuffers");
            _glGenSamplers = GetFunctionDelegate<DEL_V_IUip>("glGenSamplers");
            _glGenTextures = GetFunctionDelegate<DEL_V_IUip>("glGenTextures");
            _glGenTransformFeedbacks = GetFunctionDelegate<DEL_V_IUip>("glGenTransformFeedbacks");
            _glGenVertexArrays = GetFunctionDelegate<DEL_V_IUip>("glGenVertexArrays");
            _glGetActiveAtomicCounterBufferiv = GetFunctionDelegate<DEL_V_UiUiEAtomicCounterBufferPNameEIp>("glGetActiveAtomicCounterBufferiv");
            _glGetActiveAttrib = GetFunctionDelegate<DEL_V_UiUiIIpIpEpAttributeTypeEpCp>("glGetActiveAttrib");
            _glGetActiveSubroutineName = GetFunctionDelegate<DEL_V_UiEShaderTypeEUiIIpCp>("glGetActiveSubroutineName");
            _glGetActiveSubroutineUniformiv = GetFunctionDelegate<DEL_V_UiEShaderTypeEUiESubroutineParameterNameEIp>("glGetActiveSubroutineUniformiv");
            _glGetActiveSubroutineUniformName = GetFunctionDelegate<DEL_V_UiEShaderTypeEUiIIpCp>("glGetActiveSubroutineUniformName");
            _glGetActiveUniform = GetFunctionDelegate<DEL_V_UiUiIIpIpEpUniformTypeEpCp>("glGetActiveUniform");
            _glGetActiveUniformBlockiv = GetFunctionDelegate<DEL_V_UiUiEUniformBlockPNameEIp>("glGetActiveUniformBlockiv");
            _glGetActiveUniformBlockName = GetFunctionDelegate<DEL_V_UiUiIIpCp>("glGetActiveUniformBlockName");
            _glGetActiveUniformName = GetFunctionDelegate<DEL_V_UiUiIIpCp>("glGetActiveUniformName");
            _glGetActiveUniformsiv = GetFunctionDelegate<DEL_V_UiIUipEUniformPNameEIp>("glGetActiveUniformsiv");
            _glGetAttachedShaders = GetFunctionDelegate<DEL_V_UiIIpUip>("glGetAttachedShaders");
            _glGetAttribLocation = GetFunctionDelegate<DEL_I_UiCp>("glGetAttribLocation");
            _glGetBooleani_v = GetFunctionDelegate<DEL_V_EBufferTargetARBEUiBp>("glGetBooleani_v");
            _glGetBooleanv = GetFunctionDelegate<DEL_V_EGetPNameEBp>("glGetBooleanv");
            _glGetBufferParameteri64v = GetFunctionDelegate<DEL_V_EBufferTargetARBEEBufferPNameARBELp>("glGetBufferParameteri64v");
            _glGetBufferParameteriv = GetFunctionDelegate<DEL_V_EBufferTargetARBEEBufferPNameARBEIp>("glGetBufferParameteriv");
            _glGetBufferPointerv = GetFunctionDelegate<DEL_V_EBufferTargetARBEEBufferPointerNameARBEVpp>("glGetBufferPointerv");
            _glGetBufferSubData = GetFunctionDelegate<DEL_V_EBufferTargetARBEPPVp>("glGetBufferSubData");
            _glGetClipPlane = GetFunctionDelegate<DEL_V_EClipPlaneNameEDp>("glGetClipPlane");
            _glGetCompressedTexImage = GetFunctionDelegate<DEL_V_ETextureTargetEIVp>("glGetCompressedTexImage");
            _glGetCompressedTextureImage = GetFunctionDelegate<DEL_V_UiIIVp>("glGetCompressedTextureImage");
            _glGetCompressedTextureSubImage = GetFunctionDelegate<DEL_V_UiIIIIIIIIVp>("glGetCompressedTextureSubImage");
            _glGetDebugMessageLog = GetFunctionDelegate<DEL_Ui_UiIEpDebugSourceEpEpDebugTypeEpUipEpDebugSeverityEpIpCp>("glGetDebugMessageLog");
            _glGetDoublei_v = GetFunctionDelegate<DEL_V_EGetPNameEUiDp>("glGetDoublei_v");
            _glGetDoublev = GetFunctionDelegate<DEL_V_EGetPNameEDp>("glGetDoublev");
            _glGetError = GetFunctionDelegate<DEL_EErrorCodeE_>("glGetError");
            _glGetFloati_v = GetFunctionDelegate<DEL_V_EGetPNameEUiFp>("glGetFloati_v");
            _glGetFloatv = GetFunctionDelegate<DEL_V_EGetPNameEFp>("glGetFloatv");
            _glGetFragDataIndex = GetFunctionDelegate<DEL_I_UiCp>("glGetFragDataIndex");
            _glGetFragDataLocation = GetFunctionDelegate<DEL_I_UiCp>("glGetFragDataLocation");
            _glGetFramebufferAttachmentParameteriv = GetFunctionDelegate<DEL_V_EFramebufferTargetEEFramebufferAttachmentEEFramebufferAttachmentParameterNameEIp>("glGetFramebufferAttachmentParameteriv");
            _glGetFramebufferParameteriv = GetFunctionDelegate<DEL_V_EFramebufferTargetEEFramebufferAttachmentParameterNameEIp>("glGetFramebufferParameteriv");
            _glGetGraphicsResetStatus = GetFunctionDelegate<DEL_EGraphicsResetStatusE_>("glGetGraphicsResetStatus");
            _glGetInteger64i_v = GetFunctionDelegate<DEL_V_EGetPNameEUiLp>("glGetInteger64i_v");
            _glGetInteger64v = GetFunctionDelegate<DEL_V_EGetPNameELp>("glGetInteger64v");
            _glGetIntegeri_v = GetFunctionDelegate<DEL_V_EGetPNameEUiIp>("glGetIntegeri_v");
            _glGetIntegerv = GetFunctionDelegate<DEL_V_EGetPNameEIp>("glGetIntegerv");
            _glGetInternalformati64v = GetFunctionDelegate<DEL_V_ETextureTargetEEInternalFormatEEInternalFormatPNameEILp>("glGetInternalformati64v");
            _glGetInternalformativ = GetFunctionDelegate<DEL_V_ETextureTargetEEInternalFormatEEInternalFormatPNameEIIp>("glGetInternalformativ");
            _glGetLightfv = GetFunctionDelegate<DEL_V_ELightNameEELightParameterEFp>("glGetLightfv");
            _glGetLightiv = GetFunctionDelegate<DEL_V_ELightNameEELightParameterEIp>("glGetLightiv");
            _glGetMapdv = GetFunctionDelegate<DEL_V_EMapTargetEEGetMapQueryEDp>("glGetMapdv");
            _glGetMapfv = GetFunctionDelegate<DEL_V_EMapTargetEEGetMapQueryEFp>("glGetMapfv");
            _glGetMapiv = GetFunctionDelegate<DEL_V_EMapTargetEEGetMapQueryEIp>("glGetMapiv");
            _glGetMaterialfv = GetFunctionDelegate<DEL_V_EMaterialFaceEEMaterialParameterEFp>("glGetMaterialfv");
            _glGetMaterialiv = GetFunctionDelegate<DEL_V_EMaterialFaceEEMaterialParameterEIp>("glGetMaterialiv");
            _glGetMultisamplefv = GetFunctionDelegate<DEL_V_EGetMultisamplePNameNVEUiFp>("glGetMultisamplefv");
            _glGetNamedBufferParameteri64v = GetFunctionDelegate<DEL_V_UiEBufferPNameARBELp>("glGetNamedBufferParameteri64v");
            _glGetNamedBufferParameteriv = GetFunctionDelegate<DEL_V_UiEBufferPNameARBEIp>("glGetNamedBufferParameteriv");
            _glGetNamedBufferPointerv = GetFunctionDelegate<DEL_V_UiEBufferPointerNameARBEVpp>("glGetNamedBufferPointerv");
            _glGetNamedBufferSubData = GetFunctionDelegate<DEL_V_UiPPVp>("glGetNamedBufferSubData");
            _glGetNamedFramebufferAttachmentParameteriv = GetFunctionDelegate<DEL_V_UiEFramebufferAttachmentEEFramebufferAttachmentParameterNameEIp>("glGetNamedFramebufferAttachmentParameteriv");
            _glGetNamedFramebufferParameteriv = GetFunctionDelegate<DEL_V_UiEGetFramebufferParameterEIp>("glGetNamedFramebufferParameteriv");
            _glGetNamedRenderbufferParameteriv = GetFunctionDelegate<DEL_V_UiERenderbufferParameterNameEIp>("glGetNamedRenderbufferParameteriv");
            _glGetnColorTable = GetFunctionDelegate<DEL_V_EColorTableTargetEEPixelFormatEEPixelTypeEIVp>("glGetnColorTable");
            _glGetnCompressedTexImage = GetFunctionDelegate<DEL_V_ETextureTargetEIIVp>("glGetnCompressedTexImage");
            _glGetnConvolutionFilter = GetFunctionDelegate<DEL_V_EConvolutionTargetEEPixelFormatEEPixelTypeEIVp>("glGetnConvolutionFilter");
            _glGetnHistogram = GetFunctionDelegate<DEL_V_EHistogramTargetEBEPixelFormatEEPixelTypeEIVp>("glGetnHistogram");
            _glGetnMapdv = GetFunctionDelegate<DEL_V_EMapTargetEEMapQueryEIDp>("glGetnMapdv");
            _glGetnMapfv = GetFunctionDelegate<DEL_V_EMapTargetEEMapQueryEIFp>("glGetnMapfv");
            _glGetnMapiv = GetFunctionDelegate<DEL_V_EMapTargetEEMapQueryEIIp>("glGetnMapiv");
            _glGetnMinmax = GetFunctionDelegate<DEL_V_EMinmaxTargetEBEPixelFormatEEPixelTypeEIVp>("glGetnMinmax");
            _glGetnPixelMapfv = GetFunctionDelegate<DEL_V_EPixelMapEIFp>("glGetnPixelMapfv");
            _glGetnPixelMapuiv = GetFunctionDelegate<DEL_V_EPixelMapEIUip>("glGetnPixelMapuiv");
            _glGetnPixelMapusv = GetFunctionDelegate<DEL_V_EPixelMapEIUsp>("glGetnPixelMapusv");
            _glGetnPolygonStipple = GetFunctionDelegate<DEL_V_IByp>("glGetnPolygonStipple");
            _glGetnSeparableFilter = GetFunctionDelegate<DEL_V_ESeparableTargetEEPixelFormatEEPixelTypeEIVpIVpVp>("glGetnSeparableFilter");
            _glGetnTexImage = GetFunctionDelegate<DEL_V_ETextureTargetEIEPixelFormatEEPixelTypeEIVp>("glGetnTexImage");
            _glGetnUniformdv = GetFunctionDelegate<DEL_V_UiIIDp>("glGetnUniformdv");
            _glGetnUniformfv = GetFunctionDelegate<DEL_V_UiIIFp>("glGetnUniformfv");
            _glGetnUniformiv = GetFunctionDelegate<DEL_V_UiIIIp>("glGetnUniformiv");
            _glGetnUniformuiv = GetFunctionDelegate<DEL_V_UiIIUip>("glGetnUniformuiv");
            _glGetObjectLabel = GetFunctionDelegate<DEL_V_EObjectIdentifierEUiIIpCp>("glGetObjectLabel");
            _glGetObjectPtrLabel = GetFunctionDelegate<DEL_V_VpIIpCp>("glGetObjectPtrLabel");
            _glGetPixelMapfv = GetFunctionDelegate<DEL_V_EPixelMapEFp>("glGetPixelMapfv");
            _glGetPixelMapuiv = GetFunctionDelegate<DEL_V_EPixelMapEUip>("glGetPixelMapuiv");
            _glGetPixelMapusv = GetFunctionDelegate<DEL_V_EPixelMapEUsp>("glGetPixelMapusv");
            _glGetPointerv = GetFunctionDelegate<DEL_V_EGetPointervPNameEVpp>("glGetPointerv");
            _glGetPolygonStipple = GetFunctionDelegate<DEL_V_Byp>("glGetPolygonStipple");
            _glGetProgramBinary = GetFunctionDelegate<DEL_V_UiIIpIpVp>("glGetProgramBinary");
            _glGetProgramInfoLog = GetFunctionDelegate<DEL_V_UiIIpCp>("glGetProgramInfoLog");
            _glGetProgramInterfaceiv = GetFunctionDelegate<DEL_V_UiEProgramInterfaceEEProgramInterfacePNameEIp>("glGetProgramInterfaceiv");
            _glGetProgramiv = GetFunctionDelegate<DEL_V_UiEProgramPropertyARBEIp>("glGetProgramiv");
            _glGetProgramPipelineInfoLog = GetFunctionDelegate<DEL_V_UiIIpCp>("glGetProgramPipelineInfoLog");
            _glGetProgramPipelineiv = GetFunctionDelegate<DEL_V_UiEPipelineParameterNameEIp>("glGetProgramPipelineiv");
            _glGetProgramResourceIndex = GetFunctionDelegate<DEL_Ui_UiEProgramInterfaceECp>("glGetProgramResourceIndex");
            _glGetProgramResourceiv = GetFunctionDelegate<DEL_V_UiEProgramInterfaceEUiIEpProgramResourcePropertyEpIIpIp>("glGetProgramResourceiv");
            _glGetProgramResourceLocation = GetFunctionDelegate<DEL_I_UiEProgramInterfaceECp>("glGetProgramResourceLocation");
            _glGetProgramResourceLocationIndex = GetFunctionDelegate<DEL_I_UiEProgramInterfaceECp>("glGetProgramResourceLocationIndex");
            _glGetProgramResourceName = GetFunctionDelegate<DEL_V_UiEProgramInterfaceEUiIIpCp>("glGetProgramResourceName");
            _glGetProgramStageiv = GetFunctionDelegate<DEL_V_UiEShaderTypeEEProgramStagePNameEIp>("glGetProgramStageiv");
            _glGetQueryBufferObjecti64v = GetFunctionDelegate<DEL_V_UiUiEQueryObjectParameterNameEP>("glGetQueryBufferObjecti64v");
            _glGetQueryBufferObjectiv = GetFunctionDelegate<DEL_V_UiUiEQueryObjectParameterNameEP>("glGetQueryBufferObjectiv");
            _glGetQueryBufferObjectui64v = GetFunctionDelegate<DEL_V_UiUiEQueryObjectParameterNameEP>("glGetQueryBufferObjectui64v");
            _glGetQueryBufferObjectuiv = GetFunctionDelegate<DEL_V_UiUiEQueryObjectParameterNameEP>("glGetQueryBufferObjectuiv");
            _glGetQueryIndexediv = GetFunctionDelegate<DEL_V_EQueryTargetEUiEQueryParameterNameEIp>("glGetQueryIndexediv");
            _glGetQueryiv = GetFunctionDelegate<DEL_V_EQueryTargetEEQueryParameterNameEIp>("glGetQueryiv");
            _glGetQueryObjecti64v = GetFunctionDelegate<DEL_V_UiEQueryObjectParameterNameELp>("glGetQueryObjecti64v");
            _glGetQueryObjectiv = GetFunctionDelegate<DEL_V_UiEQueryObjectParameterNameEIp>("glGetQueryObjectiv");
            _glGetQueryObjectui64v = GetFunctionDelegate<DEL_V_UiEQueryObjectParameterNameEUlp>("glGetQueryObjectui64v");
            _glGetQueryObjectuiv = GetFunctionDelegate<DEL_V_UiEQueryObjectParameterNameEUip>("glGetQueryObjectuiv");
            _glGetRenderbufferParameteriv = GetFunctionDelegate<DEL_V_ERenderbufferTargetEERenderbufferParameterNameEIp>("glGetRenderbufferParameteriv");
            _glGetSamplerParameterfv = GetFunctionDelegate<DEL_V_UiESamplerParameterFEFp>("glGetSamplerParameterfv");
            _glGetSamplerParameterIiv = GetFunctionDelegate<DEL_V_UiESamplerParameterIEIp>("glGetSamplerParameterIiv");
            _glGetSamplerParameterIuiv = GetFunctionDelegate<DEL_V_UiESamplerParameterIEUip>("glGetSamplerParameterIuiv");
            _glGetSamplerParameteriv = GetFunctionDelegate<DEL_V_UiESamplerParameterIEIp>("glGetSamplerParameteriv");
            _glGetShaderInfoLog = GetFunctionDelegate<DEL_V_UiIIpCp>("glGetShaderInfoLog");
            _glGetShaderiv = GetFunctionDelegate<DEL_V_UiEShaderParameterNameEIp>("glGetShaderiv");
            _glGetShaderPrecisionFormat = GetFunctionDelegate<DEL_V_EShaderTypeEEPrecisionTypeEIpIp>("glGetShaderPrecisionFormat");
            _glGetShaderSource = GetFunctionDelegate<DEL_V_UiIIpCp>("glGetShaderSource");
            _glGetString = GetFunctionDelegate<DEL_Byp_EStringNameE>("glGetString");
            _glGetStringi = GetFunctionDelegate<DEL_Byp_EStringNameEUi>("glGetStringi");
            _glGetSubroutineIndex = GetFunctionDelegate<DEL_Ui_UiEShaderTypeECp>("glGetSubroutineIndex");
            _glGetSubroutineUniformLocation = GetFunctionDelegate<DEL_I_UiEShaderTypeECp>("glGetSubroutineUniformLocation");
            _glGetSynciv = GetFunctionDelegate<DEL_V_StpSyncStpESyncParameterNameEIIpIp>("glGetSynciv");
            _glGetTexEnvfv = GetFunctionDelegate<DEL_V_ETextureEnvTargetEETextureEnvParameterEFp>("glGetTexEnvfv");
            _glGetTexEnviv = GetFunctionDelegate<DEL_V_ETextureEnvTargetEETextureEnvParameterEIp>("glGetTexEnviv");
            _glGetTexGendv = GetFunctionDelegate<DEL_V_ETextureCoordNameEETextureGenParameterEDp>("glGetTexGendv");
            _glGetTexGenfv = GetFunctionDelegate<DEL_V_ETextureCoordNameEETextureGenParameterEFp>("glGetTexGenfv");
            _glGetTexGeniv = GetFunctionDelegate<DEL_V_ETextureCoordNameEETextureGenParameterEIp>("glGetTexGeniv");
            _glGetTexImage = GetFunctionDelegate<DEL_V_ETextureTargetEIEPixelFormatEEPixelTypeEVp>("glGetTexImage");
            _glGetTexLevelParameterfv = GetFunctionDelegate<DEL_V_ETextureTargetEIEGetTextureParameterEFp>("glGetTexLevelParameterfv");
            _glGetTexLevelParameteriv = GetFunctionDelegate<DEL_V_ETextureTargetEIEGetTextureParameterEIp>("glGetTexLevelParameteriv");
            _glGetTexParameterfv = GetFunctionDelegate<DEL_V_ETextureTargetEEGetTextureParameterEFp>("glGetTexParameterfv");
            _glGetTexParameterIiv = GetFunctionDelegate<DEL_V_ETextureTargetEEGetTextureParameterEIp>("glGetTexParameterIiv");
            _glGetTexParameterIuiv = GetFunctionDelegate<DEL_V_ETextureTargetEEGetTextureParameterEUip>("glGetTexParameterIuiv");
            _glGetTexParameteriv = GetFunctionDelegate<DEL_V_ETextureTargetEEGetTextureParameterEIp>("glGetTexParameteriv");
            _glGetTextureImage = GetFunctionDelegate<DEL_V_UiIEPixelFormatEEPixelTypeEIVp>("glGetTextureImage");
            _glGetTextureLevelParameterfv = GetFunctionDelegate<DEL_V_UiIEGetTextureParameterEFp>("glGetTextureLevelParameterfv");
            _glGetTextureLevelParameteriv = GetFunctionDelegate<DEL_V_UiIEGetTextureParameterEIp>("glGetTextureLevelParameteriv");
            _glGetTextureParameterfv = GetFunctionDelegate<DEL_V_UiEGetTextureParameterEFp>("glGetTextureParameterfv");
            _glGetTextureParameterIiv = GetFunctionDelegate<DEL_V_UiEGetTextureParameterEIp>("glGetTextureParameterIiv");
            _glGetTextureParameterIuiv = GetFunctionDelegate<DEL_V_UiEGetTextureParameterEUip>("glGetTextureParameterIuiv");
            _glGetTextureParameteriv = GetFunctionDelegate<DEL_V_UiEGetTextureParameterEIp>("glGetTextureParameteriv");
            _glGetTextureSubImage = GetFunctionDelegate<DEL_V_UiIIIIIIIEPixelFormatEEPixelTypeEIVp>("glGetTextureSubImage");
            _glGetTransformFeedbacki_v = GetFunctionDelegate<DEL_V_UiETransformFeedbackPNameEUiIp>("glGetTransformFeedbacki_v");
            _glGetTransformFeedbacki64_v = GetFunctionDelegate<DEL_V_UiETransformFeedbackPNameEUiLp>("glGetTransformFeedbacki64_v");
            _glGetTransformFeedbackiv = GetFunctionDelegate<DEL_V_UiETransformFeedbackPNameEIp>("glGetTransformFeedbackiv");
            _glGetTransformFeedbackVarying = GetFunctionDelegate<DEL_V_UiUiIIpIpEpAttributeTypeEpCp>("glGetTransformFeedbackVarying");
            _glGetUniformBlockIndex = GetFunctionDelegate<DEL_Ui_UiCp>("glGetUniformBlockIndex");
            _glGetUniformdv = GetFunctionDelegate<DEL_V_UiIDp>("glGetUniformdv");
            _glGetUniformfv = GetFunctionDelegate<DEL_V_UiIFp>("glGetUniformfv");
            _glGetUniformIndices = GetFunctionDelegate<DEL_V_UiICppUip>("glGetUniformIndices");
            _glGetUniformiv = GetFunctionDelegate<DEL_V_UiIIp>("glGetUniformiv");
            _glGetUniformLocation = GetFunctionDelegate<DEL_I_UiCp>("glGetUniformLocation");
            _glGetUniformSubroutineuiv = GetFunctionDelegate<DEL_V_EShaderTypeEIUip>("glGetUniformSubroutineuiv");
            _glGetUniformuiv = GetFunctionDelegate<DEL_V_UiIUip>("glGetUniformuiv");
            _glGetVertexArrayIndexed64iv = GetFunctionDelegate<DEL_V_UiUiEVertexArrayPNameELp>("glGetVertexArrayIndexed64iv");
            _glGetVertexArrayIndexediv = GetFunctionDelegate<DEL_V_UiUiEVertexArrayPNameEIp>("glGetVertexArrayIndexediv");
            _glGetVertexArrayiv = GetFunctionDelegate<DEL_V_UiEVertexArrayPNameEIp>("glGetVertexArrayiv");
            _glGetVertexAttribdv = GetFunctionDelegate<DEL_V_UiEVertexAttribPropertyARBEDp>("glGetVertexAttribdv");
            _glGetVertexAttribfv = GetFunctionDelegate<DEL_V_UiEVertexAttribPropertyARBEFp>("glGetVertexAttribfv");
            _glGetVertexAttribIiv = GetFunctionDelegate<DEL_V_UiEVertexAttribEnumEIp>("glGetVertexAttribIiv");
            _glGetVertexAttribIuiv = GetFunctionDelegate<DEL_V_UiEVertexAttribEnumEUip>("glGetVertexAttribIuiv");
            _glGetVertexAttribiv = GetFunctionDelegate<DEL_V_UiEVertexAttribPropertyARBEIp>("glGetVertexAttribiv");
            _glGetVertexAttribLdv = GetFunctionDelegate<DEL_V_UiEVertexAttribEnumEDp>("glGetVertexAttribLdv");
            _glGetVertexAttribPointerv = GetFunctionDelegate<DEL_V_UiEVertexAttribPointerPropertyARBEVpp>("glGetVertexAttribPointerv");
            _glHint = GetFunctionDelegate<DEL_V_EHintTargetEEHintModeE>("glHint");
            _glIndexd = GetFunctionDelegate<DEL_V_D>("glIndexd");
            _glIndexdv = GetFunctionDelegate<DEL_V_Dp>("glIndexdv");
            _glIndexf = GetFunctionDelegate<DEL_V_F>("glIndexf");
            _glIndexfv = GetFunctionDelegate<DEL_V_Fp>("glIndexfv");
            _glIndexi = GetFunctionDelegate<DEL_V_I>("glIndexi");
            _glIndexiv = GetFunctionDelegate<DEL_V_Ip>("glIndexiv");
            _glIndexMask = GetFunctionDelegate<DEL_V_Ui>("glIndexMask");
            _glIndexPointer = GetFunctionDelegate<DEL_V_EIndexPointerTypeEIVp>("glIndexPointer");
            _glIndexs = GetFunctionDelegate<DEL_V_S>("glIndexs");
            _glIndexsv = GetFunctionDelegate<DEL_V_Sp>("glIndexsv");
            _glIndexub = GetFunctionDelegate<DEL_V_By>("glIndexub");
            _glIndexubv = GetFunctionDelegate<DEL_V_Byp>("glIndexubv");
            _glInitNames = GetFunctionDelegate<DEL_V_>("glInitNames");
            _glInterleavedArrays = GetFunctionDelegate<DEL_V_EInterleavedArrayFormatEIVp>("glInterleavedArrays");
            _glInvalidateBufferData = GetFunctionDelegate<DEL_V_Ui>("glInvalidateBufferData");
            _glInvalidateBufferSubData = GetFunctionDelegate<DEL_V_UiPP>("glInvalidateBufferSubData");
            _glInvalidateFramebuffer = GetFunctionDelegate<DEL_V_EFramebufferTargetEIEpInvalidateFramebufferAttachmentEp>("glInvalidateFramebuffer");
            _glInvalidateNamedFramebufferData = GetFunctionDelegate<DEL_V_UiIEpFramebufferAttachmentEp>("glInvalidateNamedFramebufferData");
            _glInvalidateNamedFramebufferSubData = GetFunctionDelegate<DEL_V_UiIEpFramebufferAttachmentEpIIII>("glInvalidateNamedFramebufferSubData");
            _glInvalidateSubFramebuffer = GetFunctionDelegate<DEL_V_EFramebufferTargetEIEpInvalidateFramebufferAttachmentEpIIII>("glInvalidateSubFramebuffer");
            _glInvalidateTexImage = GetFunctionDelegate<DEL_V_UiI>("glInvalidateTexImage");
            _glInvalidateTexSubImage = GetFunctionDelegate<DEL_V_UiIIIIIII>("glInvalidateTexSubImage");
            _glIsBuffer = GetFunctionDelegate<DEL_B_Ui>("glIsBuffer");
            _glIsEnabled = GetFunctionDelegate<DEL_B_EEnableCapE>("glIsEnabled");
            _glIsEnabledi = GetFunctionDelegate<DEL_B_EEnableCapEUi>("glIsEnabledi");
            _glIsFramebuffer = GetFunctionDelegate<DEL_B_Ui>("glIsFramebuffer");
            _glIsList = GetFunctionDelegate<DEL_B_Ui>("glIsList");
            _glIsProgram = GetFunctionDelegate<DEL_B_Ui>("glIsProgram");
            _glIsProgramPipeline = GetFunctionDelegate<DEL_B_Ui>("glIsProgramPipeline");
            _glIsQuery = GetFunctionDelegate<DEL_B_Ui>("glIsQuery");
            _glIsRenderbuffer = GetFunctionDelegate<DEL_B_Ui>("glIsRenderbuffer");
            _glIsSampler = GetFunctionDelegate<DEL_B_Ui>("glIsSampler");
            _glIsShader = GetFunctionDelegate<DEL_B_Ui>("glIsShader");
            _glIsSync = GetFunctionDelegate<DEL_B_StpSyncStp>("glIsSync");
            _glIsTexture = GetFunctionDelegate<DEL_B_Ui>("glIsTexture");
            _glIsTransformFeedback = GetFunctionDelegate<DEL_B_Ui>("glIsTransformFeedback");
            _glIsVertexArray = GetFunctionDelegate<DEL_B_Ui>("glIsVertexArray");
            _glLightf = GetFunctionDelegate<DEL_V_ELightNameEELightParameterEF>("glLightf");
            _glLightfv = GetFunctionDelegate<DEL_V_ELightNameEELightParameterEFp>("glLightfv");
            _glLighti = GetFunctionDelegate<DEL_V_ELightNameEELightParameterEI>("glLighti");
            _glLightiv = GetFunctionDelegate<DEL_V_ELightNameEELightParameterEIp>("glLightiv");
            _glLightModelf = GetFunctionDelegate<DEL_V_ELightModelParameterEF>("glLightModelf");
            _glLightModelfv = GetFunctionDelegate<DEL_V_ELightModelParameterEFp>("glLightModelfv");
            _glLightModeli = GetFunctionDelegate<DEL_V_ELightModelParameterEI>("glLightModeli");
            _glLightModeliv = GetFunctionDelegate<DEL_V_ELightModelParameterEIp>("glLightModeliv");
            _glLineStipple = GetFunctionDelegate<DEL_V_IUs>("glLineStipple");
            _glLineWidth = GetFunctionDelegate<DEL_V_F>("glLineWidth");
            _glLinkProgram = GetFunctionDelegate<DEL_V_Ui>("glLinkProgram");
            _glListBase = GetFunctionDelegate<DEL_V_Ui>("glListBase");
            _glLoadIdentity = GetFunctionDelegate<DEL_V_>("glLoadIdentity");
            _glLoadMatrixd = GetFunctionDelegate<DEL_V_Dp>("glLoadMatrixd");
            _glLoadMatrixf = GetFunctionDelegate<DEL_V_Fp>("glLoadMatrixf");
            _glLoadName = GetFunctionDelegate<DEL_V_Ui>("glLoadName");
            _glLoadTransposeMatrixd = GetFunctionDelegate<DEL_V_Dp>("glLoadTransposeMatrixd");
            _glLoadTransposeMatrixf = GetFunctionDelegate<DEL_V_Fp>("glLoadTransposeMatrixf");
            _glLogicOp = GetFunctionDelegate<DEL_V_ELogicOpE>("glLogicOp");
            _glMap1d = GetFunctionDelegate<DEL_V_EMapTargetEDDIIDp>("glMap1d");
            _glMap1f = GetFunctionDelegate<DEL_V_EMapTargetEFFIIFp>("glMap1f");
            _glMap2d = GetFunctionDelegate<DEL_V_EMapTargetEDDIIDDIIDp>("glMap2d");
            _glMap2f = GetFunctionDelegate<DEL_V_EMapTargetEFFIIFFIIFp>("glMap2f");
            _glMapBuffer = GetFunctionDelegate<DEL_Vp_EBufferTargetARBEEBufferAccessARBE>("glMapBuffer");
            _glMapBufferRange = GetFunctionDelegate<DEL_Vp_EBufferTargetARBEPPEMapBufferAccessMaskE>("glMapBufferRange");
            _glMapGrid1d = GetFunctionDelegate<DEL_V_IDD>("glMapGrid1d");
            _glMapGrid1f = GetFunctionDelegate<DEL_V_IFF>("glMapGrid1f");
            _glMapGrid2d = GetFunctionDelegate<DEL_V_IDDIDD>("glMapGrid2d");
            _glMapGrid2f = GetFunctionDelegate<DEL_V_IFFIFF>("glMapGrid2f");
            _glMapNamedBuffer = GetFunctionDelegate<DEL_Vp_UiEBufferAccessARBE>("glMapNamedBuffer");
            _glMapNamedBufferRange = GetFunctionDelegate<DEL_Vp_UiPPEMapBufferAccessMaskE>("glMapNamedBufferRange");
            _glMaterialf = GetFunctionDelegate<DEL_V_EMaterialFaceEEMaterialParameterEF>("glMaterialf");
            _glMaterialfv = GetFunctionDelegate<DEL_V_EMaterialFaceEEMaterialParameterEFp>("glMaterialfv");
            _glMateriali = GetFunctionDelegate<DEL_V_EMaterialFaceEEMaterialParameterEI>("glMateriali");
            _glMaterialiv = GetFunctionDelegate<DEL_V_EMaterialFaceEEMaterialParameterEIp>("glMaterialiv");
            _glMatrixMode = GetFunctionDelegate<DEL_V_EMatrixModeE>("glMatrixMode");
            _glMemoryBarrier = GetFunctionDelegate<DEL_V_EMemoryBarrierMaskE>("glMemoryBarrier");
            _glMemoryBarrierByRegion = GetFunctionDelegate<DEL_V_EMemoryBarrierMaskE>("glMemoryBarrierByRegion");
            _glMinSampleShading = GetFunctionDelegate<DEL_V_F>("glMinSampleShading");
            _glMultiDrawArrays = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIpIpI>("glMultiDrawArrays");
            _glMultiDrawArraysIndirect = GetFunctionDelegate<DEL_V_EPrimitiveTypeEVpII>("glMultiDrawArraysIndirect");
            _glMultiDrawArraysIndirectCount = GetFunctionDelegate<DEL_V_EPrimitiveTypeEVpPII>("glMultiDrawArraysIndirectCount");
            _glMultiDrawElements = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIpEDrawElementsTypeEVppI>("glMultiDrawElements");
            _glMultiDrawElementsBaseVertex = GetFunctionDelegate<DEL_V_EPrimitiveTypeEIpEDrawElementsTypeEVppIIp>("glMultiDrawElementsBaseVertex");
            _glMultiDrawElementsIndirect = GetFunctionDelegate<DEL_V_EPrimitiveTypeEEDrawElementsTypeEVpII>("glMultiDrawElementsIndirect");
            _glMultiDrawElementsIndirectCount = GetFunctionDelegate<DEL_V_EPrimitiveTypeEEDrawElementsTypeEVpPII>("glMultiDrawElementsIndirectCount");
            _glMultiTexCoord1d = GetFunctionDelegate<DEL_V_ETextureUnitED>("glMultiTexCoord1d");
            _glMultiTexCoord1dv = GetFunctionDelegate<DEL_V_ETextureUnitEDp>("glMultiTexCoord1dv");
            _glMultiTexCoord1f = GetFunctionDelegate<DEL_V_ETextureUnitEF>("glMultiTexCoord1f");
            _glMultiTexCoord1fv = GetFunctionDelegate<DEL_V_ETextureUnitEFp>("glMultiTexCoord1fv");
            _glMultiTexCoord1i = GetFunctionDelegate<DEL_V_ETextureUnitEI>("glMultiTexCoord1i");
            _glMultiTexCoord1iv = GetFunctionDelegate<DEL_V_ETextureUnitEIp>("glMultiTexCoord1iv");
            _glMultiTexCoord1s = GetFunctionDelegate<DEL_V_ETextureUnitES>("glMultiTexCoord1s");
            _glMultiTexCoord1sv = GetFunctionDelegate<DEL_V_ETextureUnitESp>("glMultiTexCoord1sv");
            _glMultiTexCoord2d = GetFunctionDelegate<DEL_V_ETextureUnitEDD>("glMultiTexCoord2d");
            _glMultiTexCoord2dv = GetFunctionDelegate<DEL_V_ETextureUnitEDp>("glMultiTexCoord2dv");
            _glMultiTexCoord2f = GetFunctionDelegate<DEL_V_ETextureUnitEFF>("glMultiTexCoord2f");
            _glMultiTexCoord2fv = GetFunctionDelegate<DEL_V_ETextureUnitEFp>("glMultiTexCoord2fv");
            _glMultiTexCoord2i = GetFunctionDelegate<DEL_V_ETextureUnitEII>("glMultiTexCoord2i");
            _glMultiTexCoord2iv = GetFunctionDelegate<DEL_V_ETextureUnitEIp>("glMultiTexCoord2iv");
            _glMultiTexCoord2s = GetFunctionDelegate<DEL_V_ETextureUnitESS>("glMultiTexCoord2s");
            _glMultiTexCoord2sv = GetFunctionDelegate<DEL_V_ETextureUnitESp>("glMultiTexCoord2sv");
            _glMultiTexCoord3d = GetFunctionDelegate<DEL_V_ETextureUnitEDDD>("glMultiTexCoord3d");
            _glMultiTexCoord3dv = GetFunctionDelegate<DEL_V_ETextureUnitEDp>("glMultiTexCoord3dv");
            _glMultiTexCoord3f = GetFunctionDelegate<DEL_V_ETextureUnitEFFF>("glMultiTexCoord3f");
            _glMultiTexCoord3fv = GetFunctionDelegate<DEL_V_ETextureUnitEFp>("glMultiTexCoord3fv");
            _glMultiTexCoord3i = GetFunctionDelegate<DEL_V_ETextureUnitEIII>("glMultiTexCoord3i");
            _glMultiTexCoord3iv = GetFunctionDelegate<DEL_V_ETextureUnitEIp>("glMultiTexCoord3iv");
            _glMultiTexCoord3s = GetFunctionDelegate<DEL_V_ETextureUnitESSS>("glMultiTexCoord3s");
            _glMultiTexCoord3sv = GetFunctionDelegate<DEL_V_ETextureUnitESp>("glMultiTexCoord3sv");
            _glMultiTexCoord4d = GetFunctionDelegate<DEL_V_ETextureUnitEDDDD>("glMultiTexCoord4d");
            _glMultiTexCoord4dv = GetFunctionDelegate<DEL_V_ETextureUnitEDp>("glMultiTexCoord4dv");
            _glMultiTexCoord4f = GetFunctionDelegate<DEL_V_ETextureUnitEFFFF>("glMultiTexCoord4f");
            _glMultiTexCoord4fv = GetFunctionDelegate<DEL_V_ETextureUnitEFp>("glMultiTexCoord4fv");
            _glMultiTexCoord4i = GetFunctionDelegate<DEL_V_ETextureUnitEIIII>("glMultiTexCoord4i");
            _glMultiTexCoord4iv = GetFunctionDelegate<DEL_V_ETextureUnitEIp>("glMultiTexCoord4iv");
            _glMultiTexCoord4s = GetFunctionDelegate<DEL_V_ETextureUnitESSSS>("glMultiTexCoord4s");
            _glMultiTexCoord4sv = GetFunctionDelegate<DEL_V_ETextureUnitESp>("glMultiTexCoord4sv");
            _glMultiTexCoordP1ui = GetFunctionDelegate<DEL_V_ETextureUnitEETexCoordPointerTypeEUi>("glMultiTexCoordP1ui");
            _glMultiTexCoordP1uiv = GetFunctionDelegate<DEL_V_ETextureUnitEETexCoordPointerTypeEUip>("glMultiTexCoordP1uiv");
            _glMultiTexCoordP2ui = GetFunctionDelegate<DEL_V_ETextureUnitEETexCoordPointerTypeEUi>("glMultiTexCoordP2ui");
            _glMultiTexCoordP2uiv = GetFunctionDelegate<DEL_V_ETextureUnitEETexCoordPointerTypeEUip>("glMultiTexCoordP2uiv");
            _glMultiTexCoordP3ui = GetFunctionDelegate<DEL_V_ETextureUnitEETexCoordPointerTypeEUi>("glMultiTexCoordP3ui");
            _glMultiTexCoordP3uiv = GetFunctionDelegate<DEL_V_ETextureUnitEETexCoordPointerTypeEUip>("glMultiTexCoordP3uiv");
            _glMultiTexCoordP4ui = GetFunctionDelegate<DEL_V_ETextureUnitEETexCoordPointerTypeEUi>("glMultiTexCoordP4ui");
            _glMultiTexCoordP4uiv = GetFunctionDelegate<DEL_V_ETextureUnitEETexCoordPointerTypeEUip>("glMultiTexCoordP4uiv");
            _glMultMatrixd = GetFunctionDelegate<DEL_V_Dp>("glMultMatrixd");
            _glMultMatrixf = GetFunctionDelegate<DEL_V_Fp>("glMultMatrixf");
            _glMultTransposeMatrixd = GetFunctionDelegate<DEL_V_Dp>("glMultTransposeMatrixd");
            _glMultTransposeMatrixf = GetFunctionDelegate<DEL_V_Fp>("glMultTransposeMatrixf");
            _glNamedBufferData = GetFunctionDelegate<DEL_V_UiPVpEVertexBufferObjectUsageE>("glNamedBufferData");
            _glNamedBufferStorage = GetFunctionDelegate<DEL_V_UiPVpEBufferStorageMaskE>("glNamedBufferStorage");
            _glNamedBufferSubData = GetFunctionDelegate<DEL_V_UiPPVp>("glNamedBufferSubData");
            _glNamedFramebufferDrawBuffer = GetFunctionDelegate<DEL_V_UiEColorBufferE>("glNamedFramebufferDrawBuffer");
            _glNamedFramebufferDrawBuffers = GetFunctionDelegate<DEL_V_UiIEpColorBufferEp>("glNamedFramebufferDrawBuffers");
            _glNamedFramebufferParameteri = GetFunctionDelegate<DEL_V_UiEFramebufferParameterNameEI>("glNamedFramebufferParameteri");
            _glNamedFramebufferReadBuffer = GetFunctionDelegate<DEL_V_UiEColorBufferE>("glNamedFramebufferReadBuffer");
            _glNamedFramebufferRenderbuffer = GetFunctionDelegate<DEL_V_UiEFramebufferAttachmentEERenderbufferTargetEUi>("glNamedFramebufferRenderbuffer");
            _glNamedFramebufferTexture = GetFunctionDelegate<DEL_V_UiEFramebufferAttachmentEUiI>("glNamedFramebufferTexture");
            _glNamedFramebufferTextureLayer = GetFunctionDelegate<DEL_V_UiEFramebufferAttachmentEUiII>("glNamedFramebufferTextureLayer");
            _glNamedRenderbufferStorage = GetFunctionDelegate<DEL_V_UiEInternalFormatEII>("glNamedRenderbufferStorage");
            _glNamedRenderbufferStorageMultisample = GetFunctionDelegate<DEL_V_UiIEInternalFormatEII>("glNamedRenderbufferStorageMultisample");
            _glNewList = GetFunctionDelegate<DEL_V_UiEListModeE>("glNewList");
            _glNormal3b = GetFunctionDelegate<DEL_V_ByByBy>("glNormal3b");
            _glNormal3bv = GetFunctionDelegate<DEL_V_Byp>("glNormal3bv");
            _glNormal3d = GetFunctionDelegate<DEL_V_DDD>("glNormal3d");
            _glNormal3dv = GetFunctionDelegate<DEL_V_Dp>("glNormal3dv");
            _glNormal3f = GetFunctionDelegate<DEL_V_FFF>("glNormal3f");
            _glNormal3fv = GetFunctionDelegate<DEL_V_Fp>("glNormal3fv");
            _glNormal3i = GetFunctionDelegate<DEL_V_III>("glNormal3i");
            _glNormal3iv = GetFunctionDelegate<DEL_V_Ip>("glNormal3iv");
            _glNormal3s = GetFunctionDelegate<DEL_V_SSS>("glNormal3s");
            _glNormal3sv = GetFunctionDelegate<DEL_V_Sp>("glNormal3sv");
            _glNormalP3ui = GetFunctionDelegate<DEL_V_ENormalPointerTypeEUi>("glNormalP3ui");
            _glNormalP3uiv = GetFunctionDelegate<DEL_V_ENormalPointerTypeEUip>("glNormalP3uiv");
            _glNormalPointer = GetFunctionDelegate<DEL_V_ENormalPointerTypeEIVp>("glNormalPointer");
            _glObjectLabel = GetFunctionDelegate<DEL_V_EObjectIdentifierEUiICp>("glObjectLabel");
            _glObjectPtrLabel = GetFunctionDelegate<DEL_V_VpICp>("glObjectPtrLabel");
            _glOrtho = GetFunctionDelegate<DEL_V_DDDDDD>("glOrtho");
            _glPassThrough = GetFunctionDelegate<DEL_V_F>("glPassThrough");
            _glPatchParameterfv = GetFunctionDelegate<DEL_V_EPatchParameterNameEFp>("glPatchParameterfv");
            _glPatchParameteri = GetFunctionDelegate<DEL_V_EPatchParameterNameEI>("glPatchParameteri");
            _glPauseTransformFeedback = GetFunctionDelegate<DEL_V_>("glPauseTransformFeedback");
            _glPixelMapfv = GetFunctionDelegate<DEL_V_EPixelMapEIFp>("glPixelMapfv");
            _glPixelMapuiv = GetFunctionDelegate<DEL_V_EPixelMapEIUip>("glPixelMapuiv");
            _glPixelMapusv = GetFunctionDelegate<DEL_V_EPixelMapEIUsp>("glPixelMapusv");
            _glPixelStoref = GetFunctionDelegate<DEL_V_EPixelStoreParameterEF>("glPixelStoref");
            _glPixelStorei = GetFunctionDelegate<DEL_V_EPixelStoreParameterEI>("glPixelStorei");
            _glPixelTransferf = GetFunctionDelegate<DEL_V_EPixelTransferParameterEF>("glPixelTransferf");
            _glPixelTransferi = GetFunctionDelegate<DEL_V_EPixelTransferParameterEI>("glPixelTransferi");
            _glPixelZoom = GetFunctionDelegate<DEL_V_FF>("glPixelZoom");
            _glPointParameterf = GetFunctionDelegate<DEL_V_EPointParameterNameARBEF>("glPointParameterf");
            _glPointParameterfv = GetFunctionDelegate<DEL_V_EPointParameterNameARBEFp>("glPointParameterfv");
            _glPointParameteri = GetFunctionDelegate<DEL_V_EPointParameterNameARBEI>("glPointParameteri");
            _glPointParameteriv = GetFunctionDelegate<DEL_V_EPointParameterNameARBEIp>("glPointParameteriv");
            _glPointSize = GetFunctionDelegate<DEL_V_F>("glPointSize");
            _glPolygonMode = GetFunctionDelegate<DEL_V_EMaterialFaceEEPolygonModeE>("glPolygonMode");
            _glPolygonOffset = GetFunctionDelegate<DEL_V_FF>("glPolygonOffset");
            _glPolygonOffsetClamp = GetFunctionDelegate<DEL_V_FFF>("glPolygonOffsetClamp");
            _glPolygonStipple = GetFunctionDelegate<DEL_V_Byp>("glPolygonStipple");
            _glPopAttrib = GetFunctionDelegate<DEL_V_>("glPopAttrib");
            _glPopClientAttrib = GetFunctionDelegate<DEL_V_>("glPopClientAttrib");
            _glPopDebugGroup = GetFunctionDelegate<DEL_V_>("glPopDebugGroup");
            _glPopMatrix = GetFunctionDelegate<DEL_V_>("glPopMatrix");
            _glPopName = GetFunctionDelegate<DEL_V_>("glPopName");
            _glPrimitiveRestartIndex = GetFunctionDelegate<DEL_V_Ui>("glPrimitiveRestartIndex");
            _glPrioritizeTextures = GetFunctionDelegate<DEL_V_IUipFp>("glPrioritizeTextures");
            _glProgramBinary = GetFunctionDelegate<DEL_V_UiIVpI>("glProgramBinary");
            _glProgramParameteri = GetFunctionDelegate<DEL_V_UiEProgramParameterPNameEI>("glProgramParameteri");
            _glProgramUniform1d = GetFunctionDelegate<DEL_V_UiID>("glProgramUniform1d");
            _glProgramUniform1dv = GetFunctionDelegate<DEL_V_UiIIDp>("glProgramUniform1dv");
            _glProgramUniform1f = GetFunctionDelegate<DEL_V_UiIF>("glProgramUniform1f");
            _glProgramUniform1fv = GetFunctionDelegate<DEL_V_UiIIFp>("glProgramUniform1fv");
            _glProgramUniform1i = GetFunctionDelegate<DEL_V_UiII>("glProgramUniform1i");
            _glProgramUniform1iv = GetFunctionDelegate<DEL_V_UiIIIp>("glProgramUniform1iv");
            _glProgramUniform1ui = GetFunctionDelegate<DEL_V_UiIUi>("glProgramUniform1ui");
            _glProgramUniform1uiv = GetFunctionDelegate<DEL_V_UiIIUip>("glProgramUniform1uiv");
            _glProgramUniform2d = GetFunctionDelegate<DEL_V_UiIDD>("glProgramUniform2d");
            _glProgramUniform2dv = GetFunctionDelegate<DEL_V_UiIIDp>("glProgramUniform2dv");
            _glProgramUniform2f = GetFunctionDelegate<DEL_V_UiIFF>("glProgramUniform2f");
            _glProgramUniform2fv = GetFunctionDelegate<DEL_V_UiIIFp>("glProgramUniform2fv");
            _glProgramUniform2i = GetFunctionDelegate<DEL_V_UiIII>("glProgramUniform2i");
            _glProgramUniform2iv = GetFunctionDelegate<DEL_V_UiIIIp>("glProgramUniform2iv");
            _glProgramUniform2ui = GetFunctionDelegate<DEL_V_UiIUiUi>("glProgramUniform2ui");
            _glProgramUniform2uiv = GetFunctionDelegate<DEL_V_UiIIUip>("glProgramUniform2uiv");
            _glProgramUniform3d = GetFunctionDelegate<DEL_V_UiIDDD>("glProgramUniform3d");
            _glProgramUniform3dv = GetFunctionDelegate<DEL_V_UiIIDp>("glProgramUniform3dv");
            _glProgramUniform3f = GetFunctionDelegate<DEL_V_UiIFFF>("glProgramUniform3f");
            _glProgramUniform3fv = GetFunctionDelegate<DEL_V_UiIIFp>("glProgramUniform3fv");
            _glProgramUniform3i = GetFunctionDelegate<DEL_V_UiIIII>("glProgramUniform3i");
            _glProgramUniform3iv = GetFunctionDelegate<DEL_V_UiIIIp>("glProgramUniform3iv");
            _glProgramUniform3ui = GetFunctionDelegate<DEL_V_UiIUiUiUi>("glProgramUniform3ui");
            _glProgramUniform3uiv = GetFunctionDelegate<DEL_V_UiIIUip>("glProgramUniform3uiv");
            _glProgramUniform4d = GetFunctionDelegate<DEL_V_UiIDDDD>("glProgramUniform4d");
            _glProgramUniform4dv = GetFunctionDelegate<DEL_V_UiIIDp>("glProgramUniform4dv");
            _glProgramUniform4f = GetFunctionDelegate<DEL_V_UiIFFFF>("glProgramUniform4f");
            _glProgramUniform4fv = GetFunctionDelegate<DEL_V_UiIIFp>("glProgramUniform4fv");
            _glProgramUniform4i = GetFunctionDelegate<DEL_V_UiIIIII>("glProgramUniform4i");
            _glProgramUniform4iv = GetFunctionDelegate<DEL_V_UiIIIp>("glProgramUniform4iv");
            _glProgramUniform4ui = GetFunctionDelegate<DEL_V_UiIUiUiUiUi>("glProgramUniform4ui");
            _glProgramUniform4uiv = GetFunctionDelegate<DEL_V_UiIIUip>("glProgramUniform4uiv");
            _glProgramUniformMatrix2dv = GetFunctionDelegate<DEL_V_UiIIBDp>("glProgramUniformMatrix2dv");
            _glProgramUniformMatrix2fv = GetFunctionDelegate<DEL_V_UiIIBFp>("glProgramUniformMatrix2fv");
            _glProgramUniformMatrix2x3dv = GetFunctionDelegate<DEL_V_UiIIBDp>("glProgramUniformMatrix2x3dv");
            _glProgramUniformMatrix2x3fv = GetFunctionDelegate<DEL_V_UiIIBFp>("glProgramUniformMatrix2x3fv");
            _glProgramUniformMatrix2x4dv = GetFunctionDelegate<DEL_V_UiIIBDp>("glProgramUniformMatrix2x4dv");
            _glProgramUniformMatrix2x4fv = GetFunctionDelegate<DEL_V_UiIIBFp>("glProgramUniformMatrix2x4fv");
            _glProgramUniformMatrix3dv = GetFunctionDelegate<DEL_V_UiIIBDp>("glProgramUniformMatrix3dv");
            _glProgramUniformMatrix3fv = GetFunctionDelegate<DEL_V_UiIIBFp>("glProgramUniformMatrix3fv");
            _glProgramUniformMatrix3x2dv = GetFunctionDelegate<DEL_V_UiIIBDp>("glProgramUniformMatrix3x2dv");
            _glProgramUniformMatrix3x2fv = GetFunctionDelegate<DEL_V_UiIIBFp>("glProgramUniformMatrix3x2fv");
            _glProgramUniformMatrix3x4dv = GetFunctionDelegate<DEL_V_UiIIBDp>("glProgramUniformMatrix3x4dv");
            _glProgramUniformMatrix3x4fv = GetFunctionDelegate<DEL_V_UiIIBFp>("glProgramUniformMatrix3x4fv");
            _glProgramUniformMatrix4dv = GetFunctionDelegate<DEL_V_UiIIBDp>("glProgramUniformMatrix4dv");
            _glProgramUniformMatrix4fv = GetFunctionDelegate<DEL_V_UiIIBFp>("glProgramUniformMatrix4fv");
            _glProgramUniformMatrix4x2dv = GetFunctionDelegate<DEL_V_UiIIBDp>("glProgramUniformMatrix4x2dv");
            _glProgramUniformMatrix4x2fv = GetFunctionDelegate<DEL_V_UiIIBFp>("glProgramUniformMatrix4x2fv");
            _glProgramUniformMatrix4x3dv = GetFunctionDelegate<DEL_V_UiIIBDp>("glProgramUniformMatrix4x3dv");
            _glProgramUniformMatrix4x3fv = GetFunctionDelegate<DEL_V_UiIIBFp>("glProgramUniformMatrix4x3fv");
            _glProvokingVertex = GetFunctionDelegate<DEL_V_EVertexProvokingModeE>("glProvokingVertex");
            _glPushAttrib = GetFunctionDelegate<DEL_V_EAttribMaskE>("glPushAttrib");
            _glPushClientAttrib = GetFunctionDelegate<DEL_V_EClientAttribMaskE>("glPushClientAttrib");
            _glPushDebugGroup = GetFunctionDelegate<DEL_V_EDebugSourceEUiICp>("glPushDebugGroup");
            _glPushMatrix = GetFunctionDelegate<DEL_V_>("glPushMatrix");
            _glPushName = GetFunctionDelegate<DEL_V_Ui>("glPushName");
            _glQueryCounter = GetFunctionDelegate<DEL_V_UiEQueryCounterTargetE>("glQueryCounter");
            _glRasterPos2d = GetFunctionDelegate<DEL_V_DD>("glRasterPos2d");
            _glRasterPos2dv = GetFunctionDelegate<DEL_V_Dp>("glRasterPos2dv");
            _glRasterPos2f = GetFunctionDelegate<DEL_V_FF>("glRasterPos2f");
            _glRasterPos2fv = GetFunctionDelegate<DEL_V_Fp>("glRasterPos2fv");
            _glRasterPos2i = GetFunctionDelegate<DEL_V_II>("glRasterPos2i");
            _glRasterPos2iv = GetFunctionDelegate<DEL_V_Ip>("glRasterPos2iv");
            _glRasterPos2s = GetFunctionDelegate<DEL_V_SS>("glRasterPos2s");
            _glRasterPos2sv = GetFunctionDelegate<DEL_V_Sp>("glRasterPos2sv");
            _glRasterPos3d = GetFunctionDelegate<DEL_V_DDD>("glRasterPos3d");
            _glRasterPos3dv = GetFunctionDelegate<DEL_V_Dp>("glRasterPos3dv");
            _glRasterPos3f = GetFunctionDelegate<DEL_V_FFF>("glRasterPos3f");
            _glRasterPos3fv = GetFunctionDelegate<DEL_V_Fp>("glRasterPos3fv");
            _glRasterPos3i = GetFunctionDelegate<DEL_V_III>("glRasterPos3i");
            _glRasterPos3iv = GetFunctionDelegate<DEL_V_Ip>("glRasterPos3iv");
            _glRasterPos3s = GetFunctionDelegate<DEL_V_SSS>("glRasterPos3s");
            _glRasterPos3sv = GetFunctionDelegate<DEL_V_Sp>("glRasterPos3sv");
            _glRasterPos4d = GetFunctionDelegate<DEL_V_DDDD>("glRasterPos4d");
            _glRasterPos4dv = GetFunctionDelegate<DEL_V_Dp>("glRasterPos4dv");
            _glRasterPos4f = GetFunctionDelegate<DEL_V_FFFF>("glRasterPos4f");
            _glRasterPos4fv = GetFunctionDelegate<DEL_V_Fp>("glRasterPos4fv");
            _glRasterPos4i = GetFunctionDelegate<DEL_V_IIII>("glRasterPos4i");
            _glRasterPos4iv = GetFunctionDelegate<DEL_V_Ip>("glRasterPos4iv");
            _glRasterPos4s = GetFunctionDelegate<DEL_V_SSSS>("glRasterPos4s");
            _glRasterPos4sv = GetFunctionDelegate<DEL_V_Sp>("glRasterPos4sv");
            _glReadBuffer = GetFunctionDelegate<DEL_V_EReadBufferModeE>("glReadBuffer");
            _glReadnPixels = GetFunctionDelegate<DEL_V_IIIIEPixelFormatEEPixelTypeEIVp>("glReadnPixels");
            _glReadPixels = GetFunctionDelegate<DEL_V_IIIIEPixelFormatEEPixelTypeEVp>("glReadPixels");
            _glRectd = GetFunctionDelegate<DEL_V_DDDD>("glRectd");
            _glRectdv = GetFunctionDelegate<DEL_V_DpDp>("glRectdv");
            _glRectf = GetFunctionDelegate<DEL_V_FFFF>("glRectf");
            _glRectfv = GetFunctionDelegate<DEL_V_FpFp>("glRectfv");
            _glRecti = GetFunctionDelegate<DEL_V_IIII>("glRecti");
            _glRectiv = GetFunctionDelegate<DEL_V_IpIp>("glRectiv");
            _glRects = GetFunctionDelegate<DEL_V_SSSS>("glRects");
            _glRectsv = GetFunctionDelegate<DEL_V_SpSp>("glRectsv");
            _glReleaseShaderCompiler = GetFunctionDelegate<DEL_V_>("glReleaseShaderCompiler");
            _glRenderbufferStorage = GetFunctionDelegate<DEL_V_ERenderbufferTargetEEInternalFormatEII>("glRenderbufferStorage");
            _glRenderbufferStorageMultisample = GetFunctionDelegate<DEL_V_ERenderbufferTargetEIEInternalFormatEII>("glRenderbufferStorageMultisample");
            _glRenderMode = GetFunctionDelegate<DEL_I_ERenderingModeE>("glRenderMode");
            _glResumeTransformFeedback = GetFunctionDelegate<DEL_V_>("glResumeTransformFeedback");
            _glRotated = GetFunctionDelegate<DEL_V_DDDD>("glRotated");
            _glRotatef = GetFunctionDelegate<DEL_V_FFFF>("glRotatef");
            _glSampleCoverage = GetFunctionDelegate<DEL_V_FB>("glSampleCoverage");
            _glSampleMaski = GetFunctionDelegate<DEL_V_UiI>("glSampleMaski");
            _glSamplerParameterf = GetFunctionDelegate<DEL_V_UiESamplerParameterFEF>("glSamplerParameterf");
            _glSamplerParameterfv = GetFunctionDelegate<DEL_V_UiESamplerParameterFEFp>("glSamplerParameterfv");
            _glSamplerParameteri = GetFunctionDelegate<DEL_V_UiESamplerParameterIEI>("glSamplerParameteri");
            _glSamplerParameterIiv = GetFunctionDelegate<DEL_V_UiESamplerParameterIEIp>("glSamplerParameterIiv");
            _glSamplerParameterIuiv = GetFunctionDelegate<DEL_V_UiESamplerParameterIEUip>("glSamplerParameterIuiv");
            _glSamplerParameteriv = GetFunctionDelegate<DEL_V_UiESamplerParameterIEIp>("glSamplerParameteriv");
            _glScaled = GetFunctionDelegate<DEL_V_DDD>("glScaled");
            _glScalef = GetFunctionDelegate<DEL_V_FFF>("glScalef");
            _glScissor = GetFunctionDelegate<DEL_V_IIII>("glScissor");
            _glScissorArrayv = GetFunctionDelegate<DEL_V_UiIIp>("glScissorArrayv");
            _glScissorIndexed = GetFunctionDelegate<DEL_V_UiIIII>("glScissorIndexed");
            _glScissorIndexedv = GetFunctionDelegate<DEL_V_UiIp>("glScissorIndexedv");
            _glSecondaryColor3b = GetFunctionDelegate<DEL_V_ByByBy>("glSecondaryColor3b");
            _glSecondaryColor3bv = GetFunctionDelegate<DEL_V_Byp>("glSecondaryColor3bv");
            _glSecondaryColor3d = GetFunctionDelegate<DEL_V_DDD>("glSecondaryColor3d");
            _glSecondaryColor3dv = GetFunctionDelegate<DEL_V_Dp>("glSecondaryColor3dv");
            _glSecondaryColor3f = GetFunctionDelegate<DEL_V_FFF>("glSecondaryColor3f");
            _glSecondaryColor3fv = GetFunctionDelegate<DEL_V_Fp>("glSecondaryColor3fv");
            _glSecondaryColor3i = GetFunctionDelegate<DEL_V_III>("glSecondaryColor3i");
            _glSecondaryColor3iv = GetFunctionDelegate<DEL_V_Ip>("glSecondaryColor3iv");
            _glSecondaryColor3s = GetFunctionDelegate<DEL_V_SSS>("glSecondaryColor3s");
            _glSecondaryColor3sv = GetFunctionDelegate<DEL_V_Sp>("glSecondaryColor3sv");
            _glSecondaryColor3ub = GetFunctionDelegate<DEL_V_ByByBy>("glSecondaryColor3ub");
            _glSecondaryColor3ubv = GetFunctionDelegate<DEL_V_Byp>("glSecondaryColor3ubv");
            _glSecondaryColor3ui = GetFunctionDelegate<DEL_V_UiUiUi>("glSecondaryColor3ui");
            _glSecondaryColor3uiv = GetFunctionDelegate<DEL_V_Uip>("glSecondaryColor3uiv");
            _glSecondaryColor3us = GetFunctionDelegate<DEL_V_UsUsUs>("glSecondaryColor3us");
            _glSecondaryColor3usv = GetFunctionDelegate<DEL_V_Usp>("glSecondaryColor3usv");
            _glSecondaryColorP3ui = GetFunctionDelegate<DEL_V_EColorPointerTypeEUi>("glSecondaryColorP3ui");
            _glSecondaryColorP3uiv = GetFunctionDelegate<DEL_V_EColorPointerTypeEUip>("glSecondaryColorP3uiv");
            _glSecondaryColorPointer = GetFunctionDelegate<DEL_V_IEColorPointerTypeEIVp>("glSecondaryColorPointer");
            _glSelectBuffer = GetFunctionDelegate<DEL_V_IUip>("glSelectBuffer");
            _glShadeModel = GetFunctionDelegate<DEL_V_EShadingModelE>("glShadeModel");
            _glShaderBinary = GetFunctionDelegate<DEL_V_IUipEShaderBinaryFormatEVpI>("glShaderBinary");
            _glShaderSource = GetFunctionDelegate<DEL_V_UiICppIp>("glShaderSource");
            _glShaderStorageBlockBinding = GetFunctionDelegate<DEL_V_UiUiUi>("glShaderStorageBlockBinding");
            _glSpecializeShader = GetFunctionDelegate<DEL_V_UiCpUiUipUip>("glSpecializeShader");
            _glStencilFunc = GetFunctionDelegate<DEL_V_EStencilFunctionEIUi>("glStencilFunc");
            _glStencilFuncSeparate = GetFunctionDelegate<DEL_V_EStencilFaceDirectionEEStencilFunctionEIUi>("glStencilFuncSeparate");
            _glStencilMask = GetFunctionDelegate<DEL_V_Ui>("glStencilMask");
            _glStencilMaskSeparate = GetFunctionDelegate<DEL_V_EStencilFaceDirectionEUi>("glStencilMaskSeparate");
            _glStencilOp = GetFunctionDelegate<DEL_V_EStencilOpEEStencilOpEEStencilOpE>("glStencilOp");
            _glStencilOpSeparate = GetFunctionDelegate<DEL_V_EStencilFaceDirectionEEStencilOpEEStencilOpEEStencilOpE>("glStencilOpSeparate");
            _glTexBuffer = GetFunctionDelegate<DEL_V_ETextureTargetEEInternalFormatEUi>("glTexBuffer");
            _glTexBufferRange = GetFunctionDelegate<DEL_V_ETextureTargetEEInternalFormatEUiPP>("glTexBufferRange");
            _glTexCoord1d = GetFunctionDelegate<DEL_V_D>("glTexCoord1d");
            _glTexCoord1dv = GetFunctionDelegate<DEL_V_Dp>("glTexCoord1dv");
            _glTexCoord1f = GetFunctionDelegate<DEL_V_F>("glTexCoord1f");
            _glTexCoord1fv = GetFunctionDelegate<DEL_V_Fp>("glTexCoord1fv");
            _glTexCoord1i = GetFunctionDelegate<DEL_V_I>("glTexCoord1i");
            _glTexCoord1iv = GetFunctionDelegate<DEL_V_Ip>("glTexCoord1iv");
            _glTexCoord1s = GetFunctionDelegate<DEL_V_S>("glTexCoord1s");
            _glTexCoord1sv = GetFunctionDelegate<DEL_V_Sp>("glTexCoord1sv");
            _glTexCoord2d = GetFunctionDelegate<DEL_V_DD>("glTexCoord2d");
            _glTexCoord2dv = GetFunctionDelegate<DEL_V_Dp>("glTexCoord2dv");
            _glTexCoord2f = GetFunctionDelegate<DEL_V_FF>("glTexCoord2f");
            _glTexCoord2fv = GetFunctionDelegate<DEL_V_Fp>("glTexCoord2fv");
            _glTexCoord2i = GetFunctionDelegate<DEL_V_II>("glTexCoord2i");
            _glTexCoord2iv = GetFunctionDelegate<DEL_V_Ip>("glTexCoord2iv");
            _glTexCoord2s = GetFunctionDelegate<DEL_V_SS>("glTexCoord2s");
            _glTexCoord2sv = GetFunctionDelegate<DEL_V_Sp>("glTexCoord2sv");
            _glTexCoord3d = GetFunctionDelegate<DEL_V_DDD>("glTexCoord3d");
            _glTexCoord3dv = GetFunctionDelegate<DEL_V_Dp>("glTexCoord3dv");
            _glTexCoord3f = GetFunctionDelegate<DEL_V_FFF>("glTexCoord3f");
            _glTexCoord3fv = GetFunctionDelegate<DEL_V_Fp>("glTexCoord3fv");
            _glTexCoord3i = GetFunctionDelegate<DEL_V_III>("glTexCoord3i");
            _glTexCoord3iv = GetFunctionDelegate<DEL_V_Ip>("glTexCoord3iv");
            _glTexCoord3s = GetFunctionDelegate<DEL_V_SSS>("glTexCoord3s");
            _glTexCoord3sv = GetFunctionDelegate<DEL_V_Sp>("glTexCoord3sv");
            _glTexCoord4d = GetFunctionDelegate<DEL_V_DDDD>("glTexCoord4d");
            _glTexCoord4dv = GetFunctionDelegate<DEL_V_Dp>("glTexCoord4dv");
            _glTexCoord4f = GetFunctionDelegate<DEL_V_FFFF>("glTexCoord4f");
            _glTexCoord4fv = GetFunctionDelegate<DEL_V_Fp>("glTexCoord4fv");
            _glTexCoord4i = GetFunctionDelegate<DEL_V_IIII>("glTexCoord4i");
            _glTexCoord4iv = GetFunctionDelegate<DEL_V_Ip>("glTexCoord4iv");
            _glTexCoord4s = GetFunctionDelegate<DEL_V_SSSS>("glTexCoord4s");
            _glTexCoord4sv = GetFunctionDelegate<DEL_V_Sp>("glTexCoord4sv");
            _glTexCoordP1ui = GetFunctionDelegate<DEL_V_ETexCoordPointerTypeEUi>("glTexCoordP1ui");
            _glTexCoordP1uiv = GetFunctionDelegate<DEL_V_ETexCoordPointerTypeEUip>("glTexCoordP1uiv");
            _glTexCoordP2ui = GetFunctionDelegate<DEL_V_ETexCoordPointerTypeEUi>("glTexCoordP2ui");
            _glTexCoordP2uiv = GetFunctionDelegate<DEL_V_ETexCoordPointerTypeEUip>("glTexCoordP2uiv");
            _glTexCoordP3ui = GetFunctionDelegate<DEL_V_ETexCoordPointerTypeEUi>("glTexCoordP3ui");
            _glTexCoordP3uiv = GetFunctionDelegate<DEL_V_ETexCoordPointerTypeEUip>("glTexCoordP3uiv");
            _glTexCoordP4ui = GetFunctionDelegate<DEL_V_ETexCoordPointerTypeEUi>("glTexCoordP4ui");
            _glTexCoordP4uiv = GetFunctionDelegate<DEL_V_ETexCoordPointerTypeEUip>("glTexCoordP4uiv");
            _glTexCoordPointer = GetFunctionDelegate<DEL_V_IETexCoordPointerTypeEIVp>("glTexCoordPointer");
            _glTexEnvf = GetFunctionDelegate<DEL_V_ETextureEnvTargetEETextureEnvParameterEF>("glTexEnvf");
            _glTexEnvfv = GetFunctionDelegate<DEL_V_ETextureEnvTargetEETextureEnvParameterEFp>("glTexEnvfv");
            _glTexEnvi = GetFunctionDelegate<DEL_V_ETextureEnvTargetEETextureEnvParameterEI>("glTexEnvi");
            _glTexEnviv = GetFunctionDelegate<DEL_V_ETextureEnvTargetEETextureEnvParameterEIp>("glTexEnviv");
            _glTexGend = GetFunctionDelegate<DEL_V_ETextureCoordNameEETextureGenParameterED>("glTexGend");
            _glTexGendv = GetFunctionDelegate<DEL_V_ETextureCoordNameEETextureGenParameterEDp>("glTexGendv");
            _glTexGenf = GetFunctionDelegate<DEL_V_ETextureCoordNameEETextureGenParameterEF>("glTexGenf");
            _glTexGenfv = GetFunctionDelegate<DEL_V_ETextureCoordNameEETextureGenParameterEFp>("glTexGenfv");
            _glTexGeni = GetFunctionDelegate<DEL_V_ETextureCoordNameEETextureGenParameterEI>("glTexGeni");
            _glTexGeniv = GetFunctionDelegate<DEL_V_ETextureCoordNameEETextureGenParameterEIp>("glTexGeniv");
            _glTexImage1D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIEPixelFormatEEPixelTypeEVp>("glTexImage1D");
            _glTexImage2D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIIEPixelFormatEEPixelTypeEVp>("glTexImage2D");
            _glTexImage2DMultisample = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIB>("glTexImage2DMultisample");
            _glTexImage3D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIIIEPixelFormatEEPixelTypeEVp>("glTexImage3D");
            _glTexImage3DMultisample = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIIB>("glTexImage3DMultisample");
            _glTexParameterf = GetFunctionDelegate<DEL_V_ETextureTargetEETextureParameterNameEF>("glTexParameterf");
            _glTexParameterfv = GetFunctionDelegate<DEL_V_ETextureTargetEETextureParameterNameEFp>("glTexParameterfv");
            _glTexParameteri = GetFunctionDelegate<DEL_V_ETextureTargetEETextureParameterNameEI>("glTexParameteri");
            _glTexParameterIiv = GetFunctionDelegate<DEL_V_ETextureTargetEETextureParameterNameEIp>("glTexParameterIiv");
            _glTexParameterIuiv = GetFunctionDelegate<DEL_V_ETextureTargetEETextureParameterNameEUip>("glTexParameterIuiv");
            _glTexParameteriv = GetFunctionDelegate<DEL_V_ETextureTargetEETextureParameterNameEIp>("glTexParameteriv");
            _glTexStorage1D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEI>("glTexStorage1D");
            _glTexStorage2D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEII>("glTexStorage2D");
            _glTexStorage2DMultisample = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIB>("glTexStorage2DMultisample");
            _glTexStorage3D = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIII>("glTexStorage3D");
            _glTexStorage3DMultisample = GetFunctionDelegate<DEL_V_ETextureTargetEIEInternalFormatEIIIB>("glTexStorage3DMultisample");
            _glTexSubImage1D = GetFunctionDelegate<DEL_V_ETextureTargetEIIIEPixelFormatEEPixelTypeEVp>("glTexSubImage1D");
            _glTexSubImage2D = GetFunctionDelegate<DEL_V_ETextureTargetEIIIIIEPixelFormatEEPixelTypeEVp>("glTexSubImage2D");
            _glTexSubImage3D = GetFunctionDelegate<DEL_V_ETextureTargetEIIIIIIIEPixelFormatEEPixelTypeEVp>("glTexSubImage3D");
            _glTextureBarrier = GetFunctionDelegate<DEL_V_>("glTextureBarrier");
            _glTextureBuffer = GetFunctionDelegate<DEL_V_UiEInternalFormatEUi>("glTextureBuffer");
            _glTextureBufferRange = GetFunctionDelegate<DEL_V_UiEInternalFormatEUiPP>("glTextureBufferRange");
            _glTextureParameterf = GetFunctionDelegate<DEL_V_UiETextureParameterNameEF>("glTextureParameterf");
            _glTextureParameterfv = GetFunctionDelegate<DEL_V_UiETextureParameterNameEFp>("glTextureParameterfv");
            _glTextureParameteri = GetFunctionDelegate<DEL_V_UiETextureParameterNameEI>("glTextureParameteri");
            _glTextureParameterIiv = GetFunctionDelegate<DEL_V_UiETextureParameterNameEIp>("glTextureParameterIiv");
            _glTextureParameterIuiv = GetFunctionDelegate<DEL_V_UiETextureParameterNameEUip>("glTextureParameterIuiv");
            _glTextureParameteriv = GetFunctionDelegate<DEL_V_UiETextureParameterNameEIp>("glTextureParameteriv");
            _glTextureStorage1D = GetFunctionDelegate<DEL_V_UiIEInternalFormatEI>("glTextureStorage1D");
            _glTextureStorage2D = GetFunctionDelegate<DEL_V_UiIEInternalFormatEII>("glTextureStorage2D");
            _glTextureStorage2DMultisample = GetFunctionDelegate<DEL_V_UiIEInternalFormatEIIB>("glTextureStorage2DMultisample");
            _glTextureStorage3D = GetFunctionDelegate<DEL_V_UiIEInternalFormatEIII>("glTextureStorage3D");
            _glTextureStorage3DMultisample = GetFunctionDelegate<DEL_V_UiIEInternalFormatEIIIB>("glTextureStorage3DMultisample");
            _glTextureSubImage1D = GetFunctionDelegate<DEL_V_UiIIIEPixelFormatEEPixelTypeEVp>("glTextureSubImage1D");
            _glTextureSubImage2D = GetFunctionDelegate<DEL_V_UiIIIIIEPixelFormatEEPixelTypeEVp>("glTextureSubImage2D");
            _glTextureSubImage3D = GetFunctionDelegate<DEL_V_UiIIIIIIIEPixelFormatEEPixelTypeEVp>("glTextureSubImage3D");
            _glTextureView = GetFunctionDelegate<DEL_V_UiETextureTargetEUiEInternalFormatEUiUiUiUi>("glTextureView");
            _glTransformFeedbackBufferBase = GetFunctionDelegate<DEL_V_UiUiUi>("glTransformFeedbackBufferBase");
            _glTransformFeedbackBufferRange = GetFunctionDelegate<DEL_V_UiUiUiPP>("glTransformFeedbackBufferRange");
            _glTransformFeedbackVaryings = GetFunctionDelegate<DEL_V_UiICppETransformFeedbackBufferModeE>("glTransformFeedbackVaryings");
            _glTranslated = GetFunctionDelegate<DEL_V_DDD>("glTranslated");
            _glTranslatef = GetFunctionDelegate<DEL_V_FFF>("glTranslatef");
            _glUniform1d = GetFunctionDelegate<DEL_V_ID>("glUniform1d");
            _glUniform1dv = GetFunctionDelegate<DEL_V_IIDp>("glUniform1dv");
            _glUniform1f = GetFunctionDelegate<DEL_V_IF>("glUniform1f");
            _glUniform1fv = GetFunctionDelegate<DEL_V_IIFp>("glUniform1fv");
            _glUniform1i = GetFunctionDelegate<DEL_V_II>("glUniform1i");
            _glUniform1iv = GetFunctionDelegate<DEL_V_IIIp>("glUniform1iv");
            _glUniform1ui = GetFunctionDelegate<DEL_V_IUi>("glUniform1ui");
            _glUniform1uiv = GetFunctionDelegate<DEL_V_IIUip>("glUniform1uiv");
            _glUniform2d = GetFunctionDelegate<DEL_V_IDD>("glUniform2d");
            _glUniform2dv = GetFunctionDelegate<DEL_V_IIDp>("glUniform2dv");
            _glUniform2f = GetFunctionDelegate<DEL_V_IFF>("glUniform2f");
            _glUniform2fv = GetFunctionDelegate<DEL_V_IIFp>("glUniform2fv");
            _glUniform2i = GetFunctionDelegate<DEL_V_III>("glUniform2i");
            _glUniform2iv = GetFunctionDelegate<DEL_V_IIIp>("glUniform2iv");
            _glUniform2ui = GetFunctionDelegate<DEL_V_IUiUi>("glUniform2ui");
            _glUniform2uiv = GetFunctionDelegate<DEL_V_IIUip>("glUniform2uiv");
            _glUniform3d = GetFunctionDelegate<DEL_V_IDDD>("glUniform3d");
            _glUniform3dv = GetFunctionDelegate<DEL_V_IIDp>("glUniform3dv");
            _glUniform3f = GetFunctionDelegate<DEL_V_IFFF>("glUniform3f");
            _glUniform3fv = GetFunctionDelegate<DEL_V_IIFp>("glUniform3fv");
            _glUniform3i = GetFunctionDelegate<DEL_V_IIII>("glUniform3i");
            _glUniform3iv = GetFunctionDelegate<DEL_V_IIIp>("glUniform3iv");
            _glUniform3ui = GetFunctionDelegate<DEL_V_IUiUiUi>("glUniform3ui");
            _glUniform3uiv = GetFunctionDelegate<DEL_V_IIUip>("glUniform3uiv");
            _glUniform4d = GetFunctionDelegate<DEL_V_IDDDD>("glUniform4d");
            _glUniform4dv = GetFunctionDelegate<DEL_V_IIDp>("glUniform4dv");
            _glUniform4f = GetFunctionDelegate<DEL_V_IFFFF>("glUniform4f");
            _glUniform4fv = GetFunctionDelegate<DEL_V_IIFp>("glUniform4fv");
            _glUniform4i = GetFunctionDelegate<DEL_V_IIIII>("glUniform4i");
            _glUniform4iv = GetFunctionDelegate<DEL_V_IIIp>("glUniform4iv");
            _glUniform4ui = GetFunctionDelegate<DEL_V_IUiUiUiUi>("glUniform4ui");
            _glUniform4uiv = GetFunctionDelegate<DEL_V_IIUip>("glUniform4uiv");
            _glUniformBlockBinding = GetFunctionDelegate<DEL_V_UiUiUi>("glUniformBlockBinding");
            _glUniformMatrix2dv = GetFunctionDelegate<DEL_V_IIBDp>("glUniformMatrix2dv");
            _glUniformMatrix2fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix2fv");
            _glUniformMatrix2x3dv = GetFunctionDelegate<DEL_V_IIBDp>("glUniformMatrix2x3dv");
            _glUniformMatrix2x3fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix2x3fv");
            _glUniformMatrix2x4dv = GetFunctionDelegate<DEL_V_IIBDp>("glUniformMatrix2x4dv");
            _glUniformMatrix2x4fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix2x4fv");
            _glUniformMatrix3dv = GetFunctionDelegate<DEL_V_IIBDp>("glUniformMatrix3dv");
            _glUniformMatrix3fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix3fv");
            _glUniformMatrix3x2dv = GetFunctionDelegate<DEL_V_IIBDp>("glUniformMatrix3x2dv");
            _glUniformMatrix3x2fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix3x2fv");
            _glUniformMatrix3x4dv = GetFunctionDelegate<DEL_V_IIBDp>("glUniformMatrix3x4dv");
            _glUniformMatrix3x4fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix3x4fv");
            _glUniformMatrix4dv = GetFunctionDelegate<DEL_V_IIBDp>("glUniformMatrix4dv");
            _glUniformMatrix4fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix4fv");
            _glUniformMatrix4x2dv = GetFunctionDelegate<DEL_V_IIBDp>("glUniformMatrix4x2dv");
            _glUniformMatrix4x2fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix4x2fv");
            _glUniformMatrix4x3dv = GetFunctionDelegate<DEL_V_IIBDp>("glUniformMatrix4x3dv");
            _glUniformMatrix4x3fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix4x3fv");
            _glUniformSubroutinesuiv = GetFunctionDelegate<DEL_V_EShaderTypeEIUip>("glUniformSubroutinesuiv");
            _glUnmapBuffer = GetFunctionDelegate<DEL_B_EBufferTargetARBE>("glUnmapBuffer");
            _glUnmapNamedBuffer = GetFunctionDelegate<DEL_B_Ui>("glUnmapNamedBuffer");
            _glUseProgram = GetFunctionDelegate<DEL_V_Ui>("glUseProgram");
            _glUseProgramStages = GetFunctionDelegate<DEL_V_UiEUseProgramStageMaskEUi>("glUseProgramStages");
            _glValidateProgram = GetFunctionDelegate<DEL_V_Ui>("glValidateProgram");
            _glValidateProgramPipeline = GetFunctionDelegate<DEL_V_Ui>("glValidateProgramPipeline");
            _glVertex2d = GetFunctionDelegate<DEL_V_DD>("glVertex2d");
            _glVertex2dv = GetFunctionDelegate<DEL_V_Dp>("glVertex2dv");
            _glVertex2f = GetFunctionDelegate<DEL_V_FF>("glVertex2f");
            _glVertex2fv = GetFunctionDelegate<DEL_V_Fp>("glVertex2fv");
            _glVertex2i = GetFunctionDelegate<DEL_V_II>("glVertex2i");
            _glVertex2iv = GetFunctionDelegate<DEL_V_Ip>("glVertex2iv");
            _glVertex2s = GetFunctionDelegate<DEL_V_SS>("glVertex2s");
            _glVertex2sv = GetFunctionDelegate<DEL_V_Sp>("glVertex2sv");
            _glVertex3d = GetFunctionDelegate<DEL_V_DDD>("glVertex3d");
            _glVertex3dv = GetFunctionDelegate<DEL_V_Dp>("glVertex3dv");
            _glVertex3f = GetFunctionDelegate<DEL_V_FFF>("glVertex3f");
            _glVertex3fv = GetFunctionDelegate<DEL_V_Fp>("glVertex3fv");
            _glVertex3i = GetFunctionDelegate<DEL_V_III>("glVertex3i");
            _glVertex3iv = GetFunctionDelegate<DEL_V_Ip>("glVertex3iv");
            _glVertex3s = GetFunctionDelegate<DEL_V_SSS>("glVertex3s");
            _glVertex3sv = GetFunctionDelegate<DEL_V_Sp>("glVertex3sv");
            _glVertex4d = GetFunctionDelegate<DEL_V_DDDD>("glVertex4d");
            _glVertex4dv = GetFunctionDelegate<DEL_V_Dp>("glVertex4dv");
            _glVertex4f = GetFunctionDelegate<DEL_V_FFFF>("glVertex4f");
            _glVertex4fv = GetFunctionDelegate<DEL_V_Fp>("glVertex4fv");
            _glVertex4i = GetFunctionDelegate<DEL_V_IIII>("glVertex4i");
            _glVertex4iv = GetFunctionDelegate<DEL_V_Ip>("glVertex4iv");
            _glVertex4s = GetFunctionDelegate<DEL_V_SSSS>("glVertex4s");
            _glVertex4sv = GetFunctionDelegate<DEL_V_Sp>("glVertex4sv");
            _glVertexArrayAttribBinding = GetFunctionDelegate<DEL_V_UiUiUi>("glVertexArrayAttribBinding");
            _glVertexArrayAttribFormat = GetFunctionDelegate<DEL_V_UiUiIEVertexAttribTypeEBUi>("glVertexArrayAttribFormat");
            _glVertexArrayAttribIFormat = GetFunctionDelegate<DEL_V_UiUiIEVertexAttribITypeEUi>("glVertexArrayAttribIFormat");
            _glVertexArrayAttribLFormat = GetFunctionDelegate<DEL_V_UiUiIEVertexAttribLTypeEUi>("glVertexArrayAttribLFormat");
            _glVertexArrayBindingDivisor = GetFunctionDelegate<DEL_V_UiUiUi>("glVertexArrayBindingDivisor");
            _glVertexArrayElementBuffer = GetFunctionDelegate<DEL_V_UiUi>("glVertexArrayElementBuffer");
            _glVertexArrayVertexBuffer = GetFunctionDelegate<DEL_V_UiUiUiPI>("glVertexArrayVertexBuffer");
            _glVertexArrayVertexBuffers = GetFunctionDelegate<DEL_V_UiUiIUipPpIp>("glVertexArrayVertexBuffers");
            _glVertexAttrib1d = GetFunctionDelegate<DEL_V_UiD>("glVertexAttrib1d");
            _glVertexAttrib1dv = GetFunctionDelegate<DEL_V_UiDp>("glVertexAttrib1dv");
            _glVertexAttrib1f = GetFunctionDelegate<DEL_V_UiF>("glVertexAttrib1f");
            _glVertexAttrib1fv = GetFunctionDelegate<DEL_V_UiFp>("glVertexAttrib1fv");
            _glVertexAttrib1s = GetFunctionDelegate<DEL_V_UiS>("glVertexAttrib1s");
            _glVertexAttrib1sv = GetFunctionDelegate<DEL_V_UiSp>("glVertexAttrib1sv");
            _glVertexAttrib2d = GetFunctionDelegate<DEL_V_UiDD>("glVertexAttrib2d");
            _glVertexAttrib2dv = GetFunctionDelegate<DEL_V_UiDp>("glVertexAttrib2dv");
            _glVertexAttrib2f = GetFunctionDelegate<DEL_V_UiFF>("glVertexAttrib2f");
            _glVertexAttrib2fv = GetFunctionDelegate<DEL_V_UiFp>("glVertexAttrib2fv");
            _glVertexAttrib2s = GetFunctionDelegate<DEL_V_UiSS>("glVertexAttrib2s");
            _glVertexAttrib2sv = GetFunctionDelegate<DEL_V_UiSp>("glVertexAttrib2sv");
            _glVertexAttrib3d = GetFunctionDelegate<DEL_V_UiDDD>("glVertexAttrib3d");
            _glVertexAttrib3dv = GetFunctionDelegate<DEL_V_UiDp>("glVertexAttrib3dv");
            _glVertexAttrib3f = GetFunctionDelegate<DEL_V_UiFFF>("glVertexAttrib3f");
            _glVertexAttrib3fv = GetFunctionDelegate<DEL_V_UiFp>("glVertexAttrib3fv");
            _glVertexAttrib3s = GetFunctionDelegate<DEL_V_UiSSS>("glVertexAttrib3s");
            _glVertexAttrib3sv = GetFunctionDelegate<DEL_V_UiSp>("glVertexAttrib3sv");
            _glVertexAttrib4bv = GetFunctionDelegate<DEL_V_UiByp>("glVertexAttrib4bv");
            _glVertexAttrib4d = GetFunctionDelegate<DEL_V_UiDDDD>("glVertexAttrib4d");
            _glVertexAttrib4dv = GetFunctionDelegate<DEL_V_UiDp>("glVertexAttrib4dv");
            _glVertexAttrib4f = GetFunctionDelegate<DEL_V_UiFFFF>("glVertexAttrib4f");
            _glVertexAttrib4fv = GetFunctionDelegate<DEL_V_UiFp>("glVertexAttrib4fv");
            _glVertexAttrib4iv = GetFunctionDelegate<DEL_V_UiIp>("glVertexAttrib4iv");
            _glVertexAttrib4Nbv = GetFunctionDelegate<DEL_V_UiByp>("glVertexAttrib4Nbv");
            _glVertexAttrib4Niv = GetFunctionDelegate<DEL_V_UiIp>("glVertexAttrib4Niv");
            _glVertexAttrib4Nsv = GetFunctionDelegate<DEL_V_UiSp>("glVertexAttrib4Nsv");
            _glVertexAttrib4Nub = GetFunctionDelegate<DEL_V_UiByByByBy>("glVertexAttrib4Nub");
            _glVertexAttrib4Nubv = GetFunctionDelegate<DEL_V_UiByp>("glVertexAttrib4Nubv");
            _glVertexAttrib4Nuiv = GetFunctionDelegate<DEL_V_UiUip>("glVertexAttrib4Nuiv");
            _glVertexAttrib4Nusv = GetFunctionDelegate<DEL_V_UiUsp>("glVertexAttrib4Nusv");
            _glVertexAttrib4s = GetFunctionDelegate<DEL_V_UiSSSS>("glVertexAttrib4s");
            _glVertexAttrib4sv = GetFunctionDelegate<DEL_V_UiSp>("glVertexAttrib4sv");
            _glVertexAttrib4ubv = GetFunctionDelegate<DEL_V_UiByp>("glVertexAttrib4ubv");
            _glVertexAttrib4uiv = GetFunctionDelegate<DEL_V_UiUip>("glVertexAttrib4uiv");
            _glVertexAttrib4usv = GetFunctionDelegate<DEL_V_UiUsp>("glVertexAttrib4usv");
            _glVertexAttribBinding = GetFunctionDelegate<DEL_V_UiUi>("glVertexAttribBinding");
            _glVertexAttribDivisor = GetFunctionDelegate<DEL_V_UiUi>("glVertexAttribDivisor");
            _glVertexAttribFormat = GetFunctionDelegate<DEL_V_UiIEVertexAttribTypeEBUi>("glVertexAttribFormat");
            _glVertexAttribI1i = GetFunctionDelegate<DEL_V_UiI>("glVertexAttribI1i");
            _glVertexAttribI1iv = GetFunctionDelegate<DEL_V_UiIp>("glVertexAttribI1iv");
            _glVertexAttribI1ui = GetFunctionDelegate<DEL_V_UiUi>("glVertexAttribI1ui");
            _glVertexAttribI1uiv = GetFunctionDelegate<DEL_V_UiUip>("glVertexAttribI1uiv");
            _glVertexAttribI2i = GetFunctionDelegate<DEL_V_UiII>("glVertexAttribI2i");
            _glVertexAttribI2iv = GetFunctionDelegate<DEL_V_UiIp>("glVertexAttribI2iv");
            _glVertexAttribI2ui = GetFunctionDelegate<DEL_V_UiUiUi>("glVertexAttribI2ui");
            _glVertexAttribI2uiv = GetFunctionDelegate<DEL_V_UiUip>("glVertexAttribI2uiv");
            _glVertexAttribI3i = GetFunctionDelegate<DEL_V_UiIII>("glVertexAttribI3i");
            _glVertexAttribI3iv = GetFunctionDelegate<DEL_V_UiIp>("glVertexAttribI3iv");
            _glVertexAttribI3ui = GetFunctionDelegate<DEL_V_UiUiUiUi>("glVertexAttribI3ui");
            _glVertexAttribI3uiv = GetFunctionDelegate<DEL_V_UiUip>("glVertexAttribI3uiv");
            _glVertexAttribI4bv = GetFunctionDelegate<DEL_V_UiByp>("glVertexAttribI4bv");
            _glVertexAttribI4i = GetFunctionDelegate<DEL_V_UiIIII>("glVertexAttribI4i");
            _glVertexAttribI4iv = GetFunctionDelegate<DEL_V_UiIp>("glVertexAttribI4iv");
            _glVertexAttribI4sv = GetFunctionDelegate<DEL_V_UiSp>("glVertexAttribI4sv");
            _glVertexAttribI4ubv = GetFunctionDelegate<DEL_V_UiByp>("glVertexAttribI4ubv");
            _glVertexAttribI4ui = GetFunctionDelegate<DEL_V_UiUiUiUiUi>("glVertexAttribI4ui");
            _glVertexAttribI4uiv = GetFunctionDelegate<DEL_V_UiUip>("glVertexAttribI4uiv");
            _glVertexAttribI4usv = GetFunctionDelegate<DEL_V_UiUsp>("glVertexAttribI4usv");
            _glVertexAttribIFormat = GetFunctionDelegate<DEL_V_UiIEVertexAttribITypeEUi>("glVertexAttribIFormat");
            _glVertexAttribIPointer = GetFunctionDelegate<DEL_V_UiIEVertexAttribITypeEIVp>("glVertexAttribIPointer");
            _glVertexAttribL1d = GetFunctionDelegate<DEL_V_UiD>("glVertexAttribL1d");
            _glVertexAttribL1dv = GetFunctionDelegate<DEL_V_UiDp>("glVertexAttribL1dv");
            _glVertexAttribL2d = GetFunctionDelegate<DEL_V_UiDD>("glVertexAttribL2d");
            _glVertexAttribL2dv = GetFunctionDelegate<DEL_V_UiDp>("glVertexAttribL2dv");
            _glVertexAttribL3d = GetFunctionDelegate<DEL_V_UiDDD>("glVertexAttribL3d");
            _glVertexAttribL3dv = GetFunctionDelegate<DEL_V_UiDp>("glVertexAttribL3dv");
            _glVertexAttribL4d = GetFunctionDelegate<DEL_V_UiDDDD>("glVertexAttribL4d");
            _glVertexAttribL4dv = GetFunctionDelegate<DEL_V_UiDp>("glVertexAttribL4dv");
            _glVertexAttribLFormat = GetFunctionDelegate<DEL_V_UiIEVertexAttribLTypeEUi>("glVertexAttribLFormat");
            _glVertexAttribLPointer = GetFunctionDelegate<DEL_V_UiIEVertexAttribLTypeEIVp>("glVertexAttribLPointer");
            _glVertexAttribP1ui = GetFunctionDelegate<DEL_V_UiEVertexAttribPointerTypeEBUi>("glVertexAttribP1ui");
            _glVertexAttribP1uiv = GetFunctionDelegate<DEL_V_UiEVertexAttribPointerTypeEBUip>("glVertexAttribP1uiv");
            _glVertexAttribP2ui = GetFunctionDelegate<DEL_V_UiEVertexAttribPointerTypeEBUi>("glVertexAttribP2ui");
            _glVertexAttribP2uiv = GetFunctionDelegate<DEL_V_UiEVertexAttribPointerTypeEBUip>("glVertexAttribP2uiv");
            _glVertexAttribP3ui = GetFunctionDelegate<DEL_V_UiEVertexAttribPointerTypeEBUi>("glVertexAttribP3ui");
            _glVertexAttribP3uiv = GetFunctionDelegate<DEL_V_UiEVertexAttribPointerTypeEBUip>("glVertexAttribP3uiv");
            _glVertexAttribP4ui = GetFunctionDelegate<DEL_V_UiEVertexAttribPointerTypeEBUi>("glVertexAttribP4ui");
            _glVertexAttribP4uiv = GetFunctionDelegate<DEL_V_UiEVertexAttribPointerTypeEBUip>("glVertexAttribP4uiv");
            _glVertexAttribPointer = GetFunctionDelegate<DEL_V_UiIEVertexAttribPointerTypeEBIVp>("glVertexAttribPointer");
            _glVertexBindingDivisor = GetFunctionDelegate<DEL_V_UiUi>("glVertexBindingDivisor");
            _glVertexP2ui = GetFunctionDelegate<DEL_V_EVertexPointerTypeEUi>("glVertexP2ui");
            _glVertexP2uiv = GetFunctionDelegate<DEL_V_EVertexPointerTypeEUip>("glVertexP2uiv");
            _glVertexP3ui = GetFunctionDelegate<DEL_V_EVertexPointerTypeEUi>("glVertexP3ui");
            _glVertexP3uiv = GetFunctionDelegate<DEL_V_EVertexPointerTypeEUip>("glVertexP3uiv");
            _glVertexP4ui = GetFunctionDelegate<DEL_V_EVertexPointerTypeEUi>("glVertexP4ui");
            _glVertexP4uiv = GetFunctionDelegate<DEL_V_EVertexPointerTypeEUip>("glVertexP4uiv");
            _glVertexPointer = GetFunctionDelegate<DEL_V_IEVertexPointerTypeEIVp>("glVertexPointer");
            _glViewport = GetFunctionDelegate<DEL_V_IIII>("glViewport");
            _glViewportArrayv = GetFunctionDelegate<DEL_V_UiIFp>("glViewportArrayv");
            _glViewportIndexedf = GetFunctionDelegate<DEL_V_UiFFFF>("glViewportIndexedf");
            _glViewportIndexedfv = GetFunctionDelegate<DEL_V_UiFp>("glViewportIndexedfv");
            _glWaitSync = GetFunctionDelegate<DEL_V_StpSyncStpESyncBehaviorFlagsEUl>("glWaitSync");
            _glWindowPos2d = GetFunctionDelegate<DEL_V_DD>("glWindowPos2d");
            _glWindowPos2dv = GetFunctionDelegate<DEL_V_Dp>("glWindowPos2dv");
            _glWindowPos2f = GetFunctionDelegate<DEL_V_FF>("glWindowPos2f");
            _glWindowPos2fv = GetFunctionDelegate<DEL_V_Fp>("glWindowPos2fv");
            _glWindowPos2i = GetFunctionDelegate<DEL_V_II>("glWindowPos2i");
            _glWindowPos2iv = GetFunctionDelegate<DEL_V_Ip>("glWindowPos2iv");
            _glWindowPos2s = GetFunctionDelegate<DEL_V_SS>("glWindowPos2s");
            _glWindowPos2sv = GetFunctionDelegate<DEL_V_Sp>("glWindowPos2sv");
            _glWindowPos3d = GetFunctionDelegate<DEL_V_DDD>("glWindowPos3d");
            _glWindowPos3dv = GetFunctionDelegate<DEL_V_Dp>("glWindowPos3dv");
            _glWindowPos3f = GetFunctionDelegate<DEL_V_FFF>("glWindowPos3f");
            _glWindowPos3fv = GetFunctionDelegate<DEL_V_Fp>("glWindowPos3fv");
            _glWindowPos3i = GetFunctionDelegate<DEL_V_III>("glWindowPos3i");
            _glWindowPos3iv = GetFunctionDelegate<DEL_V_Ip>("glWindowPos3iv");
            _glWindowPos3s = GetFunctionDelegate<DEL_V_SSS>("glWindowPos3s");
            _glWindowPos3sv = GetFunctionDelegate<DEL_V_Sp>("glWindowPos3sv");
        }

        public static void Accum(SpiceEngine.GLFWBindings.GLEnums.AccumOp op, float value) => _glAccum(op, value);
        public static void ActiveShaderProgram(uint pipeline, uint program) => _glActiveShaderProgram(pipeline, program);
        public static void ActiveTexture(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture) => _glActiveTexture(texture);
        public static void AlphaFunc(SpiceEngine.GLFWBindings.GLEnums.AlphaFunction func, float @ref) => _glAlphaFunc(func, @ref);
        public static bool AreTexturesResident(int n, uint* textures, bool* residences) => _glAreTexturesResident(n, textures, residences);
        public static void ArrayElement(int i) => _glArrayElement(i);
        public static void AttachShader(uint program, uint shader) => _glAttachShader(program, shader);
        public static void Begin(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode) => _glBegin(mode);
        public static void BeginConditionalRender(uint id, SpiceEngine.GLFWBindings.GLEnums.ConditionalRenderMode mode) => _glBeginConditionalRender(id, mode);
        public static void BeginQuery(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, uint id) => _glBeginQuery(target, id);
        public static void BeginQueryIndexed(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, uint index, uint id) => _glBeginQueryIndexed(target, index, id);
        public static void BeginTransformFeedback(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType primitiveMode) => _glBeginTransformFeedback(primitiveMode);
        public static void BindAttribLocation(uint program, uint index, char* name) => _glBindAttribLocation(program, index, name);
        public static void BindBuffer(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, uint buffer) => _glBindBuffer(target, buffer);
        public static void BindBufferBase(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, uint index, uint buffer) => _glBindBufferBase(target, index, buffer);
        public static void BindBufferRange(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, uint index, uint buffer, IntPtr offset, IntPtr size) => _glBindBufferRange(target, index, buffer, offset, size);
        public static void BindBuffersBase(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, uint first, int count, uint* buffers) => _glBindBuffersBase(target, first, count, buffers);
        public static void BindBuffersRange(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, uint first, int count, uint* buffers, IntPtr* offsets, IntPtr* sizes) => _glBindBuffersRange(target, first, count, buffers, offsets, sizes);
        public static void BindFragDataLocation(uint program, uint color, char* name) => _glBindFragDataLocation(program, color, name);
        public static void BindFragDataLocationIndexed(uint program, uint colorNumber, uint index, char* name) => _glBindFragDataLocationIndexed(program, colorNumber, index, name);
        public static void BindFramebuffer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, uint framebuffer) => _glBindFramebuffer(target, framebuffer);
        public static void BindImageTexture(uint unit, uint texture, int level, bool layered, int layer, SpiceEngine.GLFWBindings.GLEnums.BufferAccessARB access, SpiceEngine.GLFWBindings.GLEnums.InternalFormat format) => _glBindImageTexture(unit, texture, level, layered, layer, access, format);
        public static void BindImageTextures(uint first, int count, uint* textures) => _glBindImageTextures(first, count, textures);
        public static void BindProgramPipeline(uint pipeline) => _glBindProgramPipeline(pipeline);
        public static void BindRenderbuffer(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget target, uint renderbuffer) => _glBindRenderbuffer(target, renderbuffer);
        public static void BindSampler(uint unit, uint sampler) => _glBindSampler(unit, sampler);
        public static void BindSamplers(uint first, int count, uint* samplers) => _glBindSamplers(first, count, samplers);
        public static void BindTexture(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, uint texture) => _glBindTexture(target, texture);
        public static void BindTextures(uint first, int count, uint* textures) => _glBindTextures(first, count, textures);
        public static void BindTextureUnit(uint unit, uint texture) => _glBindTextureUnit(unit, texture);
        public static void BindTransformFeedback(SpiceEngine.GLFWBindings.GLEnums.BindTransformFeedbackTarget target, uint id) => _glBindTransformFeedback(target, id);
        public static void BindVertexArray(uint array) => _glBindVertexArray(array);
        public static void BindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, int stride) => _glBindVertexBuffer(bindingindex, buffer, offset, stride);
        public static void BindVertexBuffers(uint first, int count, uint* buffers, IntPtr* offsets, int* strides) => _glBindVertexBuffers(first, count, buffers, offsets, strides);
        public static void Bitmap(int width, int height, float xorig, float yorig, float xmove, float ymove, byte* bitmap) => _glBitmap(width, height, xorig, yorig, xmove, ymove, bitmap);
        public static void BlendColor(float red, float green, float blue, float alpha) => _glBlendColor(red, green, blue, alpha);
        public static void BlendEquation(SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT mode) => _glBlendEquation(mode);
        public static void BlendEquationi(uint buf, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT mode) => _glBlendEquationi(buf, mode);
        public static void BlendEquationSeparate(SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT modeRGB, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT modeAlpha) => _glBlendEquationSeparate(modeRGB, modeAlpha);
        public static void BlendEquationSeparatei(uint buf, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT modeRGB, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT modeAlpha) => _glBlendEquationSeparatei(buf, modeRGB, modeAlpha);
        public static void BlendFunc(SpiceEngine.GLFWBindings.GLEnums.BlendingFactor sfactor, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor dfactor) => _glBlendFunc(sfactor, dfactor);
        public static void BlendFunci(uint buf, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor src, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor dst) => _glBlendFunci(buf, src, dst);
        public static void BlendFuncSeparate(SpiceEngine.GLFWBindings.GLEnums.BlendingFactor sfactorRGB, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor dfactorRGB, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor sfactorAlpha, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor dfactorAlpha) => _glBlendFuncSeparate(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);
        public static void BlendFuncSeparatei(uint buf, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor srcRGB, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor dstRGB, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor srcAlpha, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor dstAlpha) => _glBlendFuncSeparatei(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
        public static void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, SpiceEngine.GLFWBindings.GLEnums.ClearBufferMask mask, SpiceEngine.GLFWBindings.GLEnums.BlitFramebufferFilter filter) => _glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
        public static void BlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, SpiceEngine.GLFWBindings.GLEnums.ClearBufferMask mask, SpiceEngine.GLFWBindings.GLEnums.BlitFramebufferFilter filter) => _glBlitNamedFramebuffer(readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
        public static void BufferData(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, IntPtr size, void* data, SpiceEngine.GLFWBindings.GLEnums.BufferUsageARB usage) => _glBufferData(target, size, data, usage);
        public static void BufferStorage(SpiceEngine.GLFWBindings.GLEnums.BufferStorageTarget target, IntPtr size, void* data, SpiceEngine.GLFWBindings.GLEnums.BufferStorageMask flags) => _glBufferStorage(target, size, data, flags);
        public static void BufferSubData(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, IntPtr offset, IntPtr size, void* data) => _glBufferSubData(target, offset, size, data);
        public static void CallList(uint list) => _glCallList(list);
        public static void CallLists(int n, SpiceEngine.GLFWBindings.GLEnums.ListNameType type, void* lists) => _glCallLists(n, type, lists);
        public static SpiceEngine.GLFWBindings.GLEnums.FramebufferStatus CheckFramebufferStatus(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target) => _glCheckFramebufferStatus(target);
        public static SpiceEngine.GLFWBindings.GLEnums.FramebufferStatus CheckNamedFramebufferStatus(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target) => _glCheckNamedFramebufferStatus(framebuffer, target);
        public static void ClampColor(SpiceEngine.GLFWBindings.GLEnums.ClampColorTargetARB target, SpiceEngine.GLFWBindings.GLEnums.ClampColorModeARB clamp) => _glClampColor(target, clamp);
        public static void Clear(SpiceEngine.GLFWBindings.GLEnums.ClearBufferMask mask) => _glClear(mask);
        public static void ClearAccum(float red, float green, float blue, float alpha) => _glClearAccum(red, green, blue, alpha);
        public static void ClearBufferData(SpiceEngine.GLFWBindings.GLEnums.BufferStorageTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* data) => _glClearBufferData(target, internalformat, format, type, data);
        public static void ClearBufferfi(SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, float depth, int stencil) => _glClearBufferfi(buffer, drawbuffer, depth, stencil);
        public static void ClearBufferfv(SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, float* value) => _glClearBufferfv(buffer, drawbuffer, value);
        public static void ClearBufferiv(SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, int* value) => _glClearBufferiv(buffer, drawbuffer, value);
        public static void ClearBufferSubData(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, IntPtr offset, IntPtr size, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* data) => _glClearBufferSubData(target, internalformat, offset, size, format, type, data);
        public static void ClearBufferuiv(SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, uint* value) => _glClearBufferuiv(buffer, drawbuffer, value);
        public static void ClearColor(float red, float green, float blue, float alpha) => _glClearColor(red, green, blue, alpha);
        public static void ClearDepth(double depth) => _glClearDepth(depth);
        public static void ClearDepthf(float d) => _glClearDepthf(d);
        public static void ClearIndex(float c) => _glClearIndex(c);
        public static void ClearNamedBufferData(uint buffer, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* data) => _glClearNamedBufferData(buffer, internalformat, format, type, data);
        public static void ClearNamedBufferSubData(uint buffer, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, IntPtr offset, IntPtr size, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* data) => _glClearNamedBufferSubData(buffer, internalformat, offset, size, format, type, data);
        public static void ClearNamedFramebufferfi(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, float depth, int stencil) => _glClearNamedFramebufferfi(framebuffer, buffer, drawbuffer, depth, stencil);
        public static void ClearNamedFramebufferfv(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, float* value) => _glClearNamedFramebufferfv(framebuffer, buffer, drawbuffer, value);
        public static void ClearNamedFramebufferiv(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, int* value) => _glClearNamedFramebufferiv(framebuffer, buffer, drawbuffer, value);
        public static void ClearNamedFramebufferuiv(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, uint* value) => _glClearNamedFramebufferuiv(framebuffer, buffer, drawbuffer, value);
        public static void ClearStencil(int s) => _glClearStencil(s);
        public static void ClearTexImage(uint texture, int level, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* data) => _glClearTexImage(texture, level, format, type, data);
        public static void ClearTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* data) => _glClearTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, data);
        public static void ClientActiveTexture(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture) => _glClientActiveTexture(texture);
        public static SpiceEngine.GLFWBindings.GLEnums.SyncStatus ClientWaitSync(SpiceEngine.GLFWBindings.GLStructs.Sync* sync, SpiceEngine.GLFWBindings.GLEnums.SyncObjectMask flags, ulong timeout) => _glClientWaitSync(sync, flags, timeout);
        public static void ClipControl(SpiceEngine.GLFWBindings.GLEnums.ClipControlOrigin origin, SpiceEngine.GLFWBindings.GLEnums.ClipControlDepth depth) => _glClipControl(origin, depth);
        public static void ClipPlane(SpiceEngine.GLFWBindings.GLEnums.ClipPlaneName plane, double* equation) => _glClipPlane(plane, equation);
        public static void Color3b(byte red, byte green, byte blue) => _glColor3b(red, green, blue);
        public static void Color3bv(byte* v) => _glColor3bv(v);
        public static void Color3d(double red, double green, double blue) => _glColor3d(red, green, blue);
        public static void Color3dv(double* v) => _glColor3dv(v);
        public static void Color3f(float red, float green, float blue) => _glColor3f(red, green, blue);
        public static void Color3fv(float* v) => _glColor3fv(v);
        public static void Color3i(int red, int green, int blue) => _glColor3i(red, green, blue);
        public static void Color3iv(int* v) => _glColor3iv(v);
        public static void Color3s(short red, short green, short blue) => _glColor3s(red, green, blue);
        public static void Color3sv(short* v) => _glColor3sv(v);
        public static void Color3ub(byte red, byte green, byte blue) => _glColor3ub(red, green, blue);
        public static void Color3ubv(byte* v) => _glColor3ubv(v);
        public static void Color3ui(uint red, uint green, uint blue) => _glColor3ui(red, green, blue);
        public static void Color3uiv(uint* v) => _glColor3uiv(v);
        public static void Color3us(ushort red, ushort green, ushort blue) => _glColor3us(red, green, blue);
        public static void Color3usv(ushort* v) => _glColor3usv(v);
        public static void Color4b(byte red, byte green, byte blue, byte alpha) => _glColor4b(red, green, blue, alpha);
        public static void Color4bv(byte* v) => _glColor4bv(v);
        public static void Color4d(double red, double green, double blue, double alpha) => _glColor4d(red, green, blue, alpha);
        public static void Color4dv(double* v) => _glColor4dv(v);
        public static void Color4f(float red, float green, float blue, float alpha) => _glColor4f(red, green, blue, alpha);
        public static void Color4fv(float* v) => _glColor4fv(v);
        public static void Color4i(int red, int green, int blue, int alpha) => _glColor4i(red, green, blue, alpha);
        public static void Color4iv(int* v) => _glColor4iv(v);
        public static void Color4s(short red, short green, short blue, short alpha) => _glColor4s(red, green, blue, alpha);
        public static void Color4sv(short* v) => _glColor4sv(v);
        public static void Color4ub(byte red, byte green, byte blue, byte alpha) => _glColor4ub(red, green, blue, alpha);
        public static void Color4ubv(byte* v) => _glColor4ubv(v);
        public static void Color4ui(uint red, uint green, uint blue, uint alpha) => _glColor4ui(red, green, blue, alpha);
        public static void Color4uiv(uint* v) => _glColor4uiv(v);
        public static void Color4us(ushort red, ushort green, ushort blue, ushort alpha) => _glColor4us(red, green, blue, alpha);
        public static void Color4usv(ushort* v) => _glColor4usv(v);
        public static void ColorMask(bool red, bool green, bool blue, bool alpha) => _glColorMask(red, green, blue, alpha);
        public static void ColorMaski(uint index, bool r, bool g, bool b, bool a) => _glColorMaski(index, r, g, b, a);
        public static void ColorMaterial(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.ColorMaterialParameter mode) => _glColorMaterial(face, mode);
        public static void ColorP3ui(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, uint color) => _glColorP3ui(type, color);
        public static void ColorP3uiv(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, uint* color) => _glColorP3uiv(type, color);
        public static void ColorP4ui(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, uint color) => _glColorP4ui(type, color);
        public static void ColorP4uiv(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, uint* color) => _glColorP4uiv(type, color);
        public static void ColorPointer(int size, SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int stride, void* pointer) => _glColorPointer(size, type, stride, pointer);
        public static void CompileShader(uint shader) => _glCompileShader(shader);
        public static void CompressedTexImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int border, int imageSize, void* data) => _glCompressedTexImage1D(target, level, internalformat, width, border, imageSize, data);
        public static void CompressedTexImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int border, int imageSize, void* data) => _glCompressedTexImage2D(target, level, internalformat, width, height, border, imageSize, data);
        public static void CompressedTexImage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth, int border, int imageSize, void* data) => _glCompressedTexImage3D(target, level, internalformat, width, height, depth, border, imageSize, data);
        public static void CompressedTexSubImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int width, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, void* data) => _glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize, data);
        public static void CompressedTexSubImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, void* data) => _glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data);
        public static void CompressedTexSubImage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, void* data) => _glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);
        public static void CompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, void* data) => _glCompressedTextureSubImage1D(texture, level, xoffset, width, format, imageSize, data);
        public static void CompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, void* data) => _glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, imageSize, data);
        public static void CompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, void* data) => _glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);
        public static void CopyBufferSubData(SpiceEngine.GLFWBindings.GLEnums.CopyBufferSubDataTarget readTarget, SpiceEngine.GLFWBindings.GLEnums.CopyBufferSubDataTarget writeTarget, IntPtr readOffset, IntPtr writeOffset, IntPtr size) => _glCopyBufferSubData(readTarget, writeTarget, readOffset, writeOffset, size);
        public static void CopyImageSubData(uint srcName, SpiceEngine.GLFWBindings.GLEnums.CopyImageSubDataTarget srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, SpiceEngine.GLFWBindings.GLEnums.CopyImageSubDataTarget dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth) => _glCopyImageSubData(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);
        public static void CopyNamedBufferSubData(uint readBuffer, uint writeBuffer, IntPtr readOffset, IntPtr writeOffset, IntPtr size) => _glCopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size);
        public static void CopyPixels(int x, int y, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelCopyType type) => _glCopyPixels(x, y, width, height, type);
        public static void CopyTexImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int x, int y, int width, int border) => _glCopyTexImage1D(target, level, internalformat, x, y, width, border);
        public static void CopyTexImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int x, int y, int width, int height, int border) => _glCopyTexImage2D(target, level, internalformat, x, y, width, height, border);
        public static void CopyTexSubImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int x, int y, int width) => _glCopyTexSubImage1D(target, level, xoffset, x, y, width);
        public static void CopyTexSubImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => _glCopyTexSubImage2D(target, level, xoffset, yoffset, x, y, width, height);
        public static void CopyTexSubImage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => _glCopyTexSubImage3D(target, level, xoffset, yoffset, zoffset, x, y, width, height);
        public static void CopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width) => _glCopyTextureSubImage1D(texture, level, xoffset, x, y, width);
        public static void CopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) => _glCopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height);
        public static void CopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => _glCopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height);
        public static void CreateBuffers(int n, uint* buffers) => _glCreateBuffers(n, buffers);
        public static void CreateFramebuffers(int n, uint* framebuffers) => _glCreateFramebuffers(n, framebuffers);
        public static uint CreateProgramInternal() => _glCreateProgram();
        public static void CreateProgramPipelines(int n, uint* pipelines) => _glCreateProgramPipelines(n, pipelines);
        public static void CreateQueries(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, int n, uint* ids) => _glCreateQueries(target, n, ids);
        public static void CreateRenderbuffers(int n, uint* renderbuffers) => _glCreateRenderbuffers(n, renderbuffers);
        public static void CreateSamplers(int n, uint* samplers) => _glCreateSamplers(n, samplers);
        public static uint CreateShaderInternal(SpiceEngine.GLFWBindings.GLEnums.ShaderType type) => _glCreateShader(type);
        public static uint CreateShaderProgramv(SpiceEngine.GLFWBindings.GLEnums.ShaderType type, int count, char** strings) => _glCreateShaderProgramv(type, count, strings);
        public static void CreateTextures(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int n, uint* textures) => _glCreateTextures(target, n, textures);
        public static void CreateTransformFeedbacks(int n, uint* ids) => _glCreateTransformFeedbacks(n, ids);
        public static void CreateVertexArrays(int n, uint* arrays) => _glCreateVertexArrays(n, arrays);
        public static void CullFace(SpiceEngine.GLFWBindings.GLEnums.CullFaceMode mode) => _glCullFace(mode);
        public static void DebugMessageCallback(void* callback, void* userParam) => _glDebugMessageCallback(callback, userParam);
        public static void DebugMessageControl(SpiceEngine.GLFWBindings.GLEnums.DebugSource source, SpiceEngine.GLFWBindings.GLEnums.DebugType type, SpiceEngine.GLFWBindings.GLEnums.DebugSeverity severity, int count, uint* ids, bool enabled) => _glDebugMessageControl(source, type, severity, count, ids, enabled);
        public static void DebugMessageInsert(SpiceEngine.GLFWBindings.GLEnums.DebugSource source, SpiceEngine.GLFWBindings.GLEnums.DebugType type, uint id, SpiceEngine.GLFWBindings.GLEnums.DebugSeverity severity, int length, char* buf) => _glDebugMessageInsert(source, type, id, severity, length, buf);
        public static void DeleteBuffers(int n, uint* buffers) => _glDeleteBuffers(n, buffers);
        public static void DeleteFramebuffers(int n, uint* framebuffers) => _glDeleteFramebuffers(n, framebuffers);
        public static void DeleteLists(uint list, int range) => _glDeleteLists(list, range);
        public static void DeleteProgram(uint program) => _glDeleteProgram(program);
        public static void DeleteProgramPipelines(int n, uint* pipelines) => _glDeleteProgramPipelines(n, pipelines);
        public static void DeleteQueries(int n, uint* ids) => _glDeleteQueries(n, ids);
        public static void DeleteRenderbuffers(int n, uint* renderbuffers) => _glDeleteRenderbuffers(n, renderbuffers);
        public static void DeleteSamplers(int count, uint* samplers) => _glDeleteSamplers(count, samplers);
        public static void DeleteShader(uint shader) => _glDeleteShader(shader);
        public static void DeleteSync(SpiceEngine.GLFWBindings.GLStructs.Sync* sync) => _glDeleteSync(sync);
        public static void DeleteTextures(int n, uint* textures) => _glDeleteTextures(n, textures);
        public static void DeleteTransformFeedbacks(int n, uint* ids) => _glDeleteTransformFeedbacks(n, ids);
        public static void DeleteVertexArrays(int n, uint* arrays) => _glDeleteVertexArrays(n, arrays);
        public static void DepthFunc(SpiceEngine.GLFWBindings.GLEnums.DepthFunction func) => _glDepthFunc(func);
        public static void DepthMask(bool flag) => _glDepthMask(flag);
        public static void DepthRange(double n, double f) => _glDepthRange(n, f);
        public static void DepthRangeArrayv(uint first, int count, double* v) => _glDepthRangeArrayv(first, count, v);
        public static void DepthRangef(float n, float f) => _glDepthRangef(n, f);
        public static void DepthRangeIndexed(uint index, double n, double f) => _glDepthRangeIndexed(index, n, f);
        public static void DetachShader(uint program, uint shader) => _glDetachShader(program, shader);
        public static void Disable(SpiceEngine.GLFWBindings.GLEnums.EnableCap cap) => _glDisable(cap);
        public static void DisableClientState(SpiceEngine.GLFWBindings.GLEnums.EnableCap array) => _glDisableClientState(array);
        public static void Disablei(SpiceEngine.GLFWBindings.GLEnums.EnableCap target, uint index) => _glDisablei(target, index);
        public static void DisableVertexArrayAttrib(uint vaobj, uint index) => _glDisableVertexArrayAttrib(vaobj, index);
        public static void DisableVertexAttribArray(uint index) => _glDisableVertexAttribArray(index);
        public static void DispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z) => _glDispatchCompute(num_groups_x, num_groups_y, num_groups_z);
        public static void DispatchComputeIndirect(IntPtr indirect) => _glDispatchComputeIndirect(indirect);
        public static void DrawArrays(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int first, int count) => _glDrawArrays(mode, first, count);
        public static void DrawArraysIndirect(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, void* indirect) => _glDrawArraysIndirect(mode, indirect);
        public static void DrawArraysInstanced(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int first, int count, int instancecount) => _glDrawArraysInstanced(mode, first, count, instancecount);
        public static void DrawArraysInstancedBaseInstance(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int first, int count, int instancecount, uint baseinstance) => _glDrawArraysInstancedBaseInstance(mode, first, count, instancecount, baseinstance);
        public static void DrawBuffer(SpiceEngine.GLFWBindings.GLEnums.DrawBufferMode buf) => _glDrawBuffer(buf);
        public static void DrawBuffers(int n, SpiceEngine.GLFWBindings.GLEnums.DrawBufferMode* bufs) => _glDrawBuffers(n, bufs);
        public static void DrawElements(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indices) => _glDrawElements(mode, count, type, indices);
        public static void DrawElementsBaseVertex(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indices, int basevertex) => _glDrawElementsBaseVertex(mode, count, type, indices, basevertex);
        public static void DrawElementsIndirect(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indirect) => _glDrawElementsIndirect(mode, type, indirect);
        public static void DrawElementsInstanced(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indices, int instancecount) => _glDrawElementsInstanced(mode, count, type, indices, instancecount);
        public static void DrawElementsInstancedBaseInstance(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.PrimitiveType type, void* indices, int instancecount, uint baseinstance) => _glDrawElementsInstancedBaseInstance(mode, count, type, indices, instancecount, baseinstance);
        public static void DrawElementsInstancedBaseVertex(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indices, int instancecount, int basevertex) => _glDrawElementsInstancedBaseVertex(mode, count, type, indices, instancecount, basevertex);
        public static void DrawElementsInstancedBaseVertexBaseInstance(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indices, int instancecount, int basevertex, uint baseinstance) => _glDrawElementsInstancedBaseVertexBaseInstance(mode, count, type, indices, instancecount, basevertex, baseinstance);
        public static void DrawPixels(int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glDrawPixels(width, height, format, type, pixels);
        public static void DrawRangeElements(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, uint start, uint end, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indices) => _glDrawRangeElements(mode, start, end, count, type, indices);
        public static void DrawRangeElementsBaseVertex(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, uint start, uint end, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indices, int basevertex) => _glDrawRangeElementsBaseVertex(mode, start, end, count, type, indices, basevertex);
        public static void DrawTransformFeedback(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, uint id) => _glDrawTransformFeedback(mode, id);
        public static void DrawTransformFeedbackInstanced(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, uint id, int instancecount) => _glDrawTransformFeedbackInstanced(mode, id, instancecount);
        public static void DrawTransformFeedbackStream(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, uint id, uint stream) => _glDrawTransformFeedbackStream(mode, id, stream);
        public static void DrawTransformFeedbackStreamInstanced(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, uint id, uint stream, int instancecount) => _glDrawTransformFeedbackStreamInstanced(mode, id, stream, instancecount);
        public static void EdgeFlag(bool flag) => _glEdgeFlag(flag);
        public static void EdgeFlagPointer(int stride, void* pointer) => _glEdgeFlagPointer(stride, pointer);
        public static void EdgeFlagv(bool* flag) => _glEdgeFlagv(flag);
        public static void Enable(SpiceEngine.GLFWBindings.GLEnums.EnableCap cap) => _glEnable(cap);
        public static void EnableClientState(SpiceEngine.GLFWBindings.GLEnums.EnableCap array) => _glEnableClientState(array);
        public static void Enablei(SpiceEngine.GLFWBindings.GLEnums.EnableCap target, uint index) => _glEnablei(target, index);
        public static void EnableVertexArrayAttrib(uint vaobj, uint index) => _glEnableVertexArrayAttrib(vaobj, index);
        public static void EnableVertexAttribArray(uint index) => _glEnableVertexAttribArray(index);
        public static void End() => _glEnd();
        public static void EndConditionalRender() => _glEndConditionalRender();
        public static void EndList() => _glEndList();
        public static void EndQuery(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target) => _glEndQuery(target);
        public static void EndQueryIndexed(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, uint index) => _glEndQueryIndexed(target, index);
        public static void EndTransformFeedback() => _glEndTransformFeedback();
        public static void EvalCoord1d(double u) => _glEvalCoord1d(u);
        public static void EvalCoord1dv(double* u) => _glEvalCoord1dv(u);
        public static void EvalCoord1f(float u) => _glEvalCoord1f(u);
        public static void EvalCoord1fv(float* u) => _glEvalCoord1fv(u);
        public static void EvalCoord2d(double u, double v) => _glEvalCoord2d(u, v);
        public static void EvalCoord2dv(double* u) => _glEvalCoord2dv(u);
        public static void EvalCoord2f(float u, float v) => _glEvalCoord2f(u, v);
        public static void EvalCoord2fv(float* u) => _glEvalCoord2fv(u);
        public static void EvalMesh1(SpiceEngine.GLFWBindings.GLEnums.MeshMode1 mode, int i1, int i2) => _glEvalMesh1(mode, i1, i2);
        public static void EvalMesh2(SpiceEngine.GLFWBindings.GLEnums.MeshMode2 mode, int i1, int i2, int j1, int j2) => _glEvalMesh2(mode, i1, i2, j1, j2);
        public static void EvalPoint1(int i) => _glEvalPoint1(i);
        public static void EvalPoint2(int i, int j) => _glEvalPoint2(i, j);
        public static void FeedbackBuffer(int size, SpiceEngine.GLFWBindings.GLEnums.FeedbackType type, float* buffer) => _glFeedbackBuffer(size, type, buffer);
        public static SpiceEngine.GLFWBindings.GLStructs.Sync* FenceSync(SpiceEngine.GLFWBindings.GLEnums.SyncCondition condition, SpiceEngine.GLFWBindings.GLEnums.SyncBehaviorFlags flags) => _glFenceSync(condition, flags);
        public static void Finish() => _glFinish();
        public static void Flush() => _glFlush();
        public static void FlushMappedBufferRange(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, IntPtr offset, IntPtr length) => _glFlushMappedBufferRange(target, offset, length);
        public static void FlushMappedNamedBufferRange(uint buffer, IntPtr offset, IntPtr length) => _glFlushMappedNamedBufferRange(buffer, offset, length);
        public static void FogCoordd(double coord) => _glFogCoordd(coord);
        public static void FogCoorddv(double* coord) => _glFogCoorddv(coord);
        public static void FogCoordf(float coord) => _glFogCoordf(coord);
        public static void FogCoordfv(float* coord) => _glFogCoordfv(coord);
        public static void FogCoordPointer(SpiceEngine.GLFWBindings.GLEnums.FogPointerTypeEXT type, int stride, void* pointer) => _glFogCoordPointer(type, stride, pointer);
        public static void Fogf(SpiceEngine.GLFWBindings.GLEnums.FogParameter pname, float param) => _glFogf(pname, param);
        public static void Fogfv(SpiceEngine.GLFWBindings.GLEnums.FogParameter pname, float* @params) => _glFogfv(pname, @params);
        public static void Fogi(SpiceEngine.GLFWBindings.GLEnums.FogParameter pname, int param) => _glFogi(pname, param);
        public static void Fogiv(SpiceEngine.GLFWBindings.GLEnums.FogParameter pname, int* @params) => _glFogiv(pname, @params);
        public static void FramebufferParameteri(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferParameterName pname, int param) => _glFramebufferParameteri(target, pname, param);
        public static void FramebufferRenderbuffer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget renderbuffertarget, uint renderbuffer) => _glFramebufferRenderbuffer(target, attachment, renderbuffertarget, renderbuffer);
        public static void FramebufferTexture(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, uint texture, int level) => _glFramebufferTexture(target, attachment, texture, level);
        public static void FramebufferTexture1D(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.TextureTarget textarget, uint texture, int level) => _glFramebufferTexture1D(target, attachment, textarget, texture, level);
        public static void FramebufferTexture2D(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.TextureTarget textarget, uint texture, int level) => _glFramebufferTexture2D(target, attachment, textarget, texture, level);
        public static void FramebufferTexture3D(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.TextureTarget textarget, uint texture, int level, int zoffset) => _glFramebufferTexture3D(target, attachment, textarget, texture, level, zoffset);
        public static void FramebufferTextureLayer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, uint texture, int level, int layer) => _glFramebufferTextureLayer(target, attachment, texture, level, layer);
        public static void FrontFace(SpiceEngine.GLFWBindings.GLEnums.FrontFaceDirection mode) => _glFrontFace(mode);
        public static void Frustum(double left, double right, double bottom, double top, double zNear, double zFar) => _glFrustum(left, right, bottom, top, zNear, zFar);
        public static void GenBuffers(int n, uint* buffers) => _glGenBuffers(n, buffers);
        public static void GenerateMipmap(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target) => _glGenerateMipmap(target);
        public static void GenerateTextureMipmap(uint texture) => _glGenerateTextureMipmap(texture);
        public static void GenFramebuffers(int n, uint* framebuffers) => _glGenFramebuffers(n, framebuffers);
        public static uint GenListsInternal(int range) => _glGenLists(range);
        public static void GenProgramPipelines(int n, uint* pipelines) => _glGenProgramPipelines(n, pipelines);
        public static void GenQueries(int n, uint* ids) => _glGenQueries(n, ids);
        public static void GenRenderbuffers(int n, uint* renderbuffers) => _glGenRenderbuffers(n, renderbuffers);
        public static void GenSamplers(int count, uint* samplers) => _glGenSamplers(count, samplers);
        public static void GenTextures(int n, uint* textures) => _glGenTextures(n, textures);
        public static void GenTransformFeedbacks(int n, uint* ids) => _glGenTransformFeedbacks(n, ids);
        public static void GenVertexArrays(int n, uint* arrays) => _glGenVertexArrays(n, arrays);
        public static void GetActiveAtomicCounterBufferiv(uint program, uint bufferIndex, SpiceEngine.GLFWBindings.GLEnums.AtomicCounterBufferPName pname, int* @params) => _glGetActiveAtomicCounterBufferiv(program, bufferIndex, pname, @params);
        public static void GetActiveAttrib(uint program, uint index, int bufSize, int* length, int* size, SpiceEngine.GLFWBindings.GLEnums.AttributeType* type, char* name) => _glGetActiveAttrib(program, index, bufSize, length, size, type, name);
        public static void GetActiveSubroutineName(uint program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, uint index, int bufSize, int* length, char* name) => _glGetActiveSubroutineName(program, shadertype, index, bufSize, length, name);
        public static void GetActiveSubroutineUniformiv(uint program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, uint index, SpiceEngine.GLFWBindings.GLEnums.SubroutineParameterName pname, int* values) => _glGetActiveSubroutineUniformiv(program, shadertype, index, pname, values);
        public static void GetActiveSubroutineUniformName(uint program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, uint index, int bufSize, int* length, char* name) => _glGetActiveSubroutineUniformName(program, shadertype, index, bufSize, length, name);
        public static void GetActiveUniform(uint program, uint index, int bufSize, int* length, int* size, SpiceEngine.GLFWBindings.GLEnums.UniformType* type, char* name) => _glGetActiveUniform(program, index, bufSize, length, size, type, name);
        public static void GetActiveUniformBlockiv(uint program, uint uniformBlockIndex, SpiceEngine.GLFWBindings.GLEnums.UniformBlockPName pname, int* @params) => _glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, @params);
        public static void GetActiveUniformBlockName(uint program, uint uniformBlockIndex, int bufSize, int* length, char* uniformBlockName) => _glGetActiveUniformBlockName(program, uniformBlockIndex, bufSize, length, uniformBlockName);
        public static void GetActiveUniformName(uint program, uint uniformIndex, int bufSize, int* length, char* uniformName) => _glGetActiveUniformName(program, uniformIndex, bufSize, length, uniformName);
        public static void GetActiveUniformsiv(uint program, int uniformCount, uint* uniformIndices, SpiceEngine.GLFWBindings.GLEnums.UniformPName pname, int* @params) => _glGetActiveUniformsiv(program, uniformCount, uniformIndices, pname, @params);
        public static void GetAttachedShaders(uint program, int maxCount, int* count, uint* shaders) => _glGetAttachedShaders(program, maxCount, count, shaders);
        public static int GetAttribLocation(uint program, char* name) => _glGetAttribLocation(program, name);
        public static void GetBooleani_v(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, uint index, bool* data) => _glGetBooleani_v(target, index, data);
        public static void GetBooleanv(SpiceEngine.GLFWBindings.GLEnums.GetPName pname, bool* data) => _glGetBooleanv(pname, data);
        public static void GetBufferParameteri64v(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB pname, long* @params) => _glGetBufferParameteri64v(target, pname, @params);
        public static void GetBufferParameteriv(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB pname, int* @params) => _glGetBufferParameteriv(target, pname, @params);
        public static void GetBufferPointerv(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, SpiceEngine.GLFWBindings.GLEnums.BufferPointerNameARB pname, void** @params) => _glGetBufferPointerv(target, pname, @params);
        public static void GetBufferSubData(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, IntPtr offset, IntPtr size, void* data) => _glGetBufferSubData(target, offset, size, data);
        public static void GetClipPlane(SpiceEngine.GLFWBindings.GLEnums.ClipPlaneName plane, double* equation) => _glGetClipPlane(plane, equation);
        public static void GetCompressedTexImage(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, void* img) => _glGetCompressedTexImage(target, level, img);
        public static void GetCompressedTextureImage(uint texture, int level, int bufSize, void* pixels) => _glGetCompressedTextureImage(texture, level, bufSize, pixels);
        public static void GetCompressedTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, void* pixels) => _glGetCompressedTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels);
        public static uint GetDebugMessageLog(uint count, int bufSize, SpiceEngine.GLFWBindings.GLEnums.DebugSource* sources, SpiceEngine.GLFWBindings.GLEnums.DebugType* types, uint* ids, SpiceEngine.GLFWBindings.GLEnums.DebugSeverity* severities, int* lengths, char* messageLog) => _glGetDebugMessageLog(count, bufSize, sources, types, ids, severities, lengths, messageLog);
        public static void GetDoublei_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, uint index, double* data) => _glGetDoublei_v(target, index, data);
        public static void GetDoublev(SpiceEngine.GLFWBindings.GLEnums.GetPName pname, double* data) => _glGetDoublev(pname, data);
        public static SpiceEngine.GLFWBindings.GLEnums.ErrorCode GetError() => _glGetError();
        public static void GetFloati_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, uint index, float* data) => _glGetFloati_v(target, index, data);
        public static void GetFloatv(SpiceEngine.GLFWBindings.GLEnums.GetPName pname, float* data) => _glGetFloatv(pname, data);
        public static int GetFragDataIndex(uint program, char* name) => _glGetFragDataIndex(program, name);
        public static int GetFragDataLocation(uint program, char* name) => _glGetFragDataLocation(program, name);
        public static void GetFramebufferAttachmentParameteriv(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName pname, int* @params) => _glGetFramebufferAttachmentParameteriv(target, attachment, pname, @params);
        public static void GetFramebufferParameteriv(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName pname, int* @params) => _glGetFramebufferParameteriv(target, pname, @params);
        public static SpiceEngine.GLFWBindings.GLEnums.GraphicsResetStatus GetGraphicsResetStatus() => _glGetGraphicsResetStatus();
        public static void GetInteger64i_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, uint index, long* data) => _glGetInteger64i_v(target, index, data);
        public static void GetInteger64v(SpiceEngine.GLFWBindings.GLEnums.GetPName pname, long* data) => _glGetInteger64v(pname, data);
        public static void GetIntegeri_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, uint index, int* data) => _glGetIntegeri_v(target, index, data);
        public static void GetIntegerv(SpiceEngine.GLFWBindings.GLEnums.GetPName pname, int* data) => _glGetIntegerv(pname, data);
        public static void GetInternalformati64v(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, SpiceEngine.GLFWBindings.GLEnums.InternalFormatPName pname, int count, long* @params) => _glGetInternalformati64v(target, internalformat, pname, count, @params);
        public static void GetInternalformativ(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, SpiceEngine.GLFWBindings.GLEnums.InternalFormatPName pname, int count, int* @params) => _glGetInternalformativ(target, internalformat, pname, count, @params);
        public static void GetLightfv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, float* @params) => _glGetLightfv(light, pname, @params);
        public static void GetLightiv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, int* @params) => _glGetLightiv(light, pname, @params);
        public static void GetMapdv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery query, double* v) => _glGetMapdv(target, query, v);
        public static void GetMapfv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery query, float* v) => _glGetMapfv(target, query, v);
        public static void GetMapiv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery query, int* v) => _glGetMapiv(target, query, v);
        public static void GetMaterialfv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, float* @params) => _glGetMaterialfv(face, pname, @params);
        public static void GetMaterialiv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, int* @params) => _glGetMaterialiv(face, pname, @params);
        public static void GetMultisamplefv(SpiceEngine.GLFWBindings.GLEnums.GetMultisamplePNameNV pname, uint index, float* val) => _glGetMultisamplefv(pname, index, val);
        public static void GetNamedBufferParameteri64v(uint buffer, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB pname, long* @params) => _glGetNamedBufferParameteri64v(buffer, pname, @params);
        public static void GetNamedBufferParameteriv(uint buffer, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB pname, int* @params) => _glGetNamedBufferParameteriv(buffer, pname, @params);
        public static void GetNamedBufferPointerv(uint buffer, SpiceEngine.GLFWBindings.GLEnums.BufferPointerNameARB pname, void** @params) => _glGetNamedBufferPointerv(buffer, pname, @params);
        public static void GetNamedBufferSubData(uint buffer, IntPtr offset, IntPtr size, void* data) => _glGetNamedBufferSubData(buffer, offset, size, data);
        public static void GetNamedFramebufferAttachmentParameteriv(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName pname, int* @params) => _glGetNamedFramebufferAttachmentParameteriv(framebuffer, attachment, pname, @params);
        public static void GetNamedFramebufferParameteriv(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.GetFramebufferParameter pname, int* param) => _glGetNamedFramebufferParameteriv(framebuffer, pname, param);
        public static void GetNamedRenderbufferParameteriv(uint renderbuffer, SpiceEngine.GLFWBindings.GLEnums.RenderbufferParameterName pname, int* @params) => _glGetNamedRenderbufferParameteriv(renderbuffer, pname, @params);
        public static void GetnColorTable(SpiceEngine.GLFWBindings.GLEnums.ColorTableTarget target, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, void* table) => _glGetnColorTable(target, format, type, bufSize, table);
        public static void GetnCompressedTexImage(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int lod, int bufSize, void* pixels) => _glGetnCompressedTexImage(target, lod, bufSize, pixels);
        public static void GetnConvolutionFilter(SpiceEngine.GLFWBindings.GLEnums.ConvolutionTarget target, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, void* image) => _glGetnConvolutionFilter(target, format, type, bufSize, image);
        public static void GetnHistogram(SpiceEngine.GLFWBindings.GLEnums.HistogramTarget target, bool reset, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, void* values) => _glGetnHistogram(target, reset, format, type, bufSize, values);
        public static void GetnMapdv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.MapQuery query, int bufSize, double* v) => _glGetnMapdv(target, query, bufSize, v);
        public static void GetnMapfv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.MapQuery query, int bufSize, float* v) => _glGetnMapfv(target, query, bufSize, v);
        public static void GetnMapiv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.MapQuery query, int bufSize, int* v) => _glGetnMapiv(target, query, bufSize, v);
        public static void GetnMinmax(SpiceEngine.GLFWBindings.GLEnums.MinmaxTarget target, bool reset, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, void* values) => _glGetnMinmax(target, reset, format, type, bufSize, values);
        public static void GetnPixelMapfv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int bufSize, float* values) => _glGetnPixelMapfv(map, bufSize, values);
        public static void GetnPixelMapuiv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int bufSize, uint* values) => _glGetnPixelMapuiv(map, bufSize, values);
        public static void GetnPixelMapusv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int bufSize, ushort* values) => _glGetnPixelMapusv(map, bufSize, values);
        public static void GetnPolygonStipple(int bufSize, byte* pattern) => _glGetnPolygonStipple(bufSize, pattern);
        public static void GetnSeparableFilter(SpiceEngine.GLFWBindings.GLEnums.SeparableTarget target, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int rowBufSize, void* row, int columnBufSize, void* column, void* span) => _glGetnSeparableFilter(target, format, type, rowBufSize, row, columnBufSize, column, span);
        public static void GetnTexImage(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, void* pixels) => _glGetnTexImage(target, level, format, type, bufSize, pixels);
        public static void GetnUniformdv(uint program, int location, int bufSize, double* @params) => _glGetnUniformdv(program, location, bufSize, @params);
        public static void GetnUniformfv(uint program, int location, int bufSize, float* @params) => _glGetnUniformfv(program, location, bufSize, @params);
        public static void GetnUniformiv(uint program, int location, int bufSize, int* @params) => _glGetnUniformiv(program, location, bufSize, @params);
        public static void GetnUniformuiv(uint program, int location, int bufSize, uint* @params) => _glGetnUniformuiv(program, location, bufSize, @params);
        public static void GetObjectLabel(SpiceEngine.GLFWBindings.GLEnums.ObjectIdentifier identifier, uint name, int bufSize, int* length, char* label) => _glGetObjectLabel(identifier, name, bufSize, length, label);
        public static void GetObjectPtrLabel(void* ptr, int bufSize, int* length, char* label) => _glGetObjectPtrLabel(ptr, bufSize, length, label);
        public static void GetPixelMapfv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, float* values) => _glGetPixelMapfv(map, values);
        public static void GetPixelMapuiv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, uint* values) => _glGetPixelMapuiv(map, values);
        public static void GetPixelMapusv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, ushort* values) => _glGetPixelMapusv(map, values);
        public static void GetPointerv(SpiceEngine.GLFWBindings.GLEnums.GetPointervPName pname, void** @params) => _glGetPointerv(pname, @params);
        public static void GetPolygonStipple(byte* mask) => _glGetPolygonStipple(mask);
        public static void GetProgramBinary(uint program, int bufSize, int* length, int* binaryFormat, void* binary) => _glGetProgramBinary(program, bufSize, length, binaryFormat, binary);
        public static void GetProgramInfoLog(uint program, int bufSize, int* length, char* infoLog) => _glGetProgramInfoLog(program, bufSize, length, infoLog);
        public static void GetProgramInterfaceiv(uint program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, SpiceEngine.GLFWBindings.GLEnums.ProgramInterfacePName pname, int* @params) => _glGetProgramInterfaceiv(program, programInterface, pname, @params);
        public static void GetProgramiv(uint program, SpiceEngine.GLFWBindings.GLEnums.ProgramPropertyARB pname, int* @params) => _glGetProgramiv(program, pname, @params);
        public static void GetProgramPipelineInfoLog(uint pipeline, int bufSize, int* length, char* infoLog) => _glGetProgramPipelineInfoLog(pipeline, bufSize, length, infoLog);
        public static void GetProgramPipelineiv(uint pipeline, SpiceEngine.GLFWBindings.GLEnums.PipelineParameterName pname, int* @params) => _glGetProgramPipelineiv(pipeline, pname, @params);
        public static uint GetProgramResourceIndex(uint program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, char* name) => _glGetProgramResourceIndex(program, programInterface, name);
        public static void GetProgramResourceiv(uint program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, uint index, int propCount, SpiceEngine.GLFWBindings.GLEnums.ProgramResourceProperty* props, int count, int* length, int* @params) => _glGetProgramResourceiv(program, programInterface, index, propCount, props, count, length, @params);
        public static int GetProgramResourceLocation(uint program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, char* name) => _glGetProgramResourceLocation(program, programInterface, name);
        public static int GetProgramResourceLocationIndex(uint program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, char* name) => _glGetProgramResourceLocationIndex(program, programInterface, name);
        public static void GetProgramResourceName(uint program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, uint index, int bufSize, int* length, char* name) => _glGetProgramResourceName(program, programInterface, index, bufSize, length, name);
        public static void GetProgramStageiv(uint program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, SpiceEngine.GLFWBindings.GLEnums.ProgramStagePName pname, int* values) => _glGetProgramStageiv(program, shadertype, pname, values);
        public static void GetQueryBufferObjecti64v(uint id, uint buffer, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, IntPtr offset) => _glGetQueryBufferObjecti64v(id, buffer, pname, offset);
        public static void GetQueryBufferObjectiv(uint id, uint buffer, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, IntPtr offset) => _glGetQueryBufferObjectiv(id, buffer, pname, offset);
        public static void GetQueryBufferObjectui64v(uint id, uint buffer, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, IntPtr offset) => _glGetQueryBufferObjectui64v(id, buffer, pname, offset);
        public static void GetQueryBufferObjectuiv(uint id, uint buffer, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, IntPtr offset) => _glGetQueryBufferObjectuiv(id, buffer, pname, offset);
        public static void GetQueryIndexediv(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, uint index, SpiceEngine.GLFWBindings.GLEnums.QueryParameterName pname, int* @params) => _glGetQueryIndexediv(target, index, pname, @params);
        public static void GetQueryiv(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, SpiceEngine.GLFWBindings.GLEnums.QueryParameterName pname, int* @params) => _glGetQueryiv(target, pname, @params);
        public static void GetQueryObjecti64v(uint id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, long* @params) => _glGetQueryObjecti64v(id, pname, @params);
        public static void GetQueryObjectiv(uint id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, int* @params) => _glGetQueryObjectiv(id, pname, @params);
        public static void GetQueryObjectui64v(uint id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, ulong* @params) => _glGetQueryObjectui64v(id, pname, @params);
        public static void GetQueryObjectuiv(uint id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, uint* @params) => _glGetQueryObjectuiv(id, pname, @params);
        public static void GetRenderbufferParameteriv(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget target, SpiceEngine.GLFWBindings.GLEnums.RenderbufferParameterName pname, int* @params) => _glGetRenderbufferParameteriv(target, pname, @params);
        public static void GetSamplerParameterfv(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterF pname, float* @params) => _glGetSamplerParameterfv(sampler, pname, @params);
        public static void GetSamplerParameterIiv(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int* @params) => _glGetSamplerParameterIiv(sampler, pname, @params);
        public static void GetSamplerParameterIuiv(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, uint* @params) => _glGetSamplerParameterIuiv(sampler, pname, @params);
        public static void GetSamplerParameteriv(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int* @params) => _glGetSamplerParameteriv(sampler, pname, @params);
        public static void GetShaderInfoLog(uint shader, int bufSize, int* length, char* infoLog) => _glGetShaderInfoLog(shader, bufSize, length, infoLog);
        public static void GetShaderiv(uint shader, SpiceEngine.GLFWBindings.GLEnums.ShaderParameterName pname, int* @params) => _glGetShaderiv(shader, pname, @params);
        public static void GetShaderPrecisionFormat(SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, SpiceEngine.GLFWBindings.GLEnums.PrecisionType precisiontype, int* range, int* precision) => _glGetShaderPrecisionFormat(shadertype, precisiontype, range, precision);
        public static void GetShaderSource(uint shader, int bufSize, int* length, char* source) => _glGetShaderSource(shader, bufSize, length, source);
        public static byte* GetString(SpiceEngine.GLFWBindings.GLEnums.StringName name) => _glGetString(name);
        public static byte* GetStringi(SpiceEngine.GLFWBindings.GLEnums.StringName name, uint index) => _glGetStringi(name, index);
        public static uint GetSubroutineIndex(uint program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, char* name) => _glGetSubroutineIndex(program, shadertype, name);
        public static int GetSubroutineUniformLocation(uint program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, char* name) => _glGetSubroutineUniformLocation(program, shadertype, name);
        public static void GetSynciv(SpiceEngine.GLFWBindings.GLStructs.Sync* sync, SpiceEngine.GLFWBindings.GLEnums.SyncParameterName pname, int count, int* length, int* values) => _glGetSynciv(sync, pname, count, length, values);
        public static void GetTexEnvfv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, float* @params) => _glGetTexEnvfv(target, pname, @params);
        public static void GetTexEnviv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, int* @params) => _glGetTexEnviv(target, pname, @params);
        public static void GetTexGendv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, double* @params) => _glGetTexGendv(coord, pname, @params);
        public static void GetTexGenfv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, float* @params) => _glGetTexGenfv(coord, pname, @params);
        public static void GetTexGeniv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, int* @params) => _glGetTexGeniv(coord, pname, @params);
        public static void GetTexImage(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glGetTexImage(target, level, format, type, pixels);
        public static void GetTexLevelParameterfv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, float* @params) => _glGetTexLevelParameterfv(target, level, pname, @params);
        public static void GetTexLevelParameteriv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int* @params) => _glGetTexLevelParameteriv(target, level, pname, @params);
        public static void GetTexParameterfv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, float* @params) => _glGetTexParameterfv(target, pname, @params);
        public static void GetTexParameterIiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int* @params) => _glGetTexParameterIiv(target, pname, @params);
        public static void GetTexParameterIuiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, uint* @params) => _glGetTexParameterIuiv(target, pname, @params);
        public static void GetTexParameteriv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int* @params) => _glGetTexParameteriv(target, pname, @params);
        public static void GetTextureImage(uint texture, int level, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, void* pixels) => _glGetTextureImage(texture, level, format, type, bufSize, pixels);
        public static void GetTextureLevelParameterfv(uint texture, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, float* @params) => _glGetTextureLevelParameterfv(texture, level, pname, @params);
        public static void GetTextureLevelParameteriv(uint texture, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int* @params) => _glGetTextureLevelParameteriv(texture, level, pname, @params);
        public static void GetTextureParameterfv(uint texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, float* @params) => _glGetTextureParameterfv(texture, pname, @params);
        public static void GetTextureParameterIiv(uint texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int* @params) => _glGetTextureParameterIiv(texture, pname, @params);
        public static void GetTextureParameterIuiv(uint texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, uint* @params) => _glGetTextureParameterIuiv(texture, pname, @params);
        public static void GetTextureParameteriv(uint texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int* @params) => _glGetTextureParameteriv(texture, pname, @params);
        public static void GetTextureSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, void* pixels) => _glGetTextureSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels);
        public static void GetTransformFeedbacki_v(uint xfb, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName pname, uint index, int* param) => _glGetTransformFeedbacki_v(xfb, pname, index, param);
        public static void GetTransformFeedbacki64_v(uint xfb, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName pname, uint index, long* param) => _glGetTransformFeedbacki64_v(xfb, pname, index, param);
        public static void GetTransformFeedbackiv(uint xfb, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName pname, int* param) => _glGetTransformFeedbackiv(xfb, pname, param);
        public static void GetTransformFeedbackVarying(uint program, uint index, int bufSize, int* length, int* size, SpiceEngine.GLFWBindings.GLEnums.AttributeType* type, char* name) => _glGetTransformFeedbackVarying(program, index, bufSize, length, size, type, name);
        public static uint GetUniformBlockIndex(uint program, char* uniformBlockName) => _glGetUniformBlockIndex(program, uniformBlockName);
        public static void GetUniformdv(uint program, int location, double* @params) => _glGetUniformdv(program, location, @params);
        public static void GetUniformfv(uint program, int location, float* @params) => _glGetUniformfv(program, location, @params);
        public static void GetUniformIndices(uint program, int uniformCount, char** uniformNames, uint* uniformIndices) => _glGetUniformIndices(program, uniformCount, uniformNames, uniformIndices);
        public static void GetUniformiv(uint program, int location, int* @params) => _glGetUniformiv(program, location, @params);
        public static int GetUniformLocation(uint program, char* name) => _glGetUniformLocation(program, name);
        public static void GetUniformSubroutineuiv(SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, int location, uint* @params) => _glGetUniformSubroutineuiv(shadertype, location, @params);
        public static void GetUniformuiv(uint program, int location, uint* @params) => _glGetUniformuiv(program, location, @params);
        public static void GetVertexArrayIndexed64iv(uint vaobj, uint index, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName pname, long* param) => _glGetVertexArrayIndexed64iv(vaobj, index, pname, param);
        public static void GetVertexArrayIndexediv(uint vaobj, uint index, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName pname, int* param) => _glGetVertexArrayIndexediv(vaobj, index, pname, param);
        public static void GetVertexArrayiv(uint vaobj, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName pname, int* param) => _glGetVertexArrayiv(vaobj, pname, param);
        public static void GetVertexAttribdv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB pname, double* @params) => _glGetVertexAttribdv(index, pname, @params);
        public static void GetVertexAttribfv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB pname, float* @params) => _glGetVertexAttribfv(index, pname, @params);
        public static void GetVertexAttribIiv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum pname, int* @params) => _glGetVertexAttribIiv(index, pname, @params);
        public static void GetVertexAttribIuiv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum pname, uint* @params) => _glGetVertexAttribIuiv(index, pname, @params);
        public static void GetVertexAttribiv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB pname, int* @params) => _glGetVertexAttribiv(index, pname, @params);
        public static void GetVertexAttribLdv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum pname, double* @params) => _glGetVertexAttribLdv(index, pname, @params);
        public static void GetVertexAttribPointerv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerPropertyARB pname, void** pointer) => _glGetVertexAttribPointerv(index, pname, pointer);
        public static void Hint(SpiceEngine.GLFWBindings.GLEnums.HintTarget target, SpiceEngine.GLFWBindings.GLEnums.HintMode mode) => _glHint(target, mode);
        public static void Indexd(double c) => _glIndexd(c);
        public static void Indexdv(double* c) => _glIndexdv(c);
        public static void Indexf(float c) => _glIndexf(c);
        public static void Indexfv(float* c) => _glIndexfv(c);
        public static void Indexi(int c) => _glIndexi(c);
        public static void Indexiv(int* c) => _glIndexiv(c);
        public static void IndexMask(uint mask) => _glIndexMask(mask);
        public static void IndexPointer(SpiceEngine.GLFWBindings.GLEnums.IndexPointerType type, int stride, void* pointer) => _glIndexPointer(type, stride, pointer);
        public static void Indexs(short c) => _glIndexs(c);
        public static void Indexsv(short* c) => _glIndexsv(c);
        public static void Indexub(byte c) => _glIndexub(c);
        public static void Indexubv(byte* c) => _glIndexubv(c);
        public static void InitNames() => _glInitNames();
        public static void InterleavedArrays(SpiceEngine.GLFWBindings.GLEnums.InterleavedArrayFormat format, int stride, void* pointer) => _glInterleavedArrays(format, stride, pointer);
        public static void InvalidateBufferData(uint buffer) => _glInvalidateBufferData(buffer);
        public static void InvalidateBufferSubData(uint buffer, IntPtr offset, IntPtr length) => _glInvalidateBufferSubData(buffer, offset, length);
        public static void InvalidateFramebuffer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, int numAttachments, SpiceEngine.GLFWBindings.GLEnums.InvalidateFramebufferAttachment* attachments) => _glInvalidateFramebuffer(target, numAttachments, attachments);
        public static void InvalidateNamedFramebufferData(uint framebuffer, int numAttachments, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment* attachments) => _glInvalidateNamedFramebufferData(framebuffer, numAttachments, attachments);
        public static void InvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment* attachments, int x, int y, int width, int height) => _glInvalidateNamedFramebufferSubData(framebuffer, numAttachments, attachments, x, y, width, height);
        public static void InvalidateSubFramebuffer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, int numAttachments, SpiceEngine.GLFWBindings.GLEnums.InvalidateFramebufferAttachment* attachments, int x, int y, int width, int height) => _glInvalidateSubFramebuffer(target, numAttachments, attachments, x, y, width, height);
        public static void InvalidateTexImage(uint texture, int level) => _glInvalidateTexImage(texture, level);
        public static void InvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth) => _glInvalidateTexSubImage(texture, level, xoffset, yoffset, zoffset, width, height, depth);
        public static bool IsBuffer(uint buffer) => _glIsBuffer(buffer);
        public static bool IsEnabled(SpiceEngine.GLFWBindings.GLEnums.EnableCap cap) => _glIsEnabled(cap);
        public static bool IsEnabledi(SpiceEngine.GLFWBindings.GLEnums.EnableCap target, uint index) => _glIsEnabledi(target, index);
        public static bool IsFramebuffer(uint framebuffer) => _glIsFramebuffer(framebuffer);
        public static bool IsList(uint list) => _glIsList(list);
        public static bool IsProgram(uint program) => _glIsProgram(program);
        public static bool IsProgramPipeline(uint pipeline) => _glIsProgramPipeline(pipeline);
        public static bool IsQuery(uint id) => _glIsQuery(id);
        public static bool IsRenderbuffer(uint renderbuffer) => _glIsRenderbuffer(renderbuffer);
        public static bool IsSampler(uint sampler) => _glIsSampler(sampler);
        public static bool IsShader(uint shader) => _glIsShader(shader);
        public static bool IsSync(SpiceEngine.GLFWBindings.GLStructs.Sync* sync) => _glIsSync(sync);
        public static bool IsTexture(uint texture) => _glIsTexture(texture);
        public static bool IsTransformFeedback(uint id) => _glIsTransformFeedback(id);
        public static bool IsVertexArray(uint array) => _glIsVertexArray(array);
        public static void Lightf(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, float param) => _glLightf(light, pname, param);
        public static void Lightfv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, float* @params) => _glLightfv(light, pname, @params);
        public static void Lighti(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, int param) => _glLighti(light, pname, param);
        public static void Lightiv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, int* @params) => _glLightiv(light, pname, @params);
        public static void LightModelf(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter pname, float param) => _glLightModelf(pname, param);
        public static void LightModelfv(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter pname, float* @params) => _glLightModelfv(pname, @params);
        public static void LightModeli(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter pname, int param) => _glLightModeli(pname, param);
        public static void LightModeliv(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter pname, int* @params) => _glLightModeliv(pname, @params);
        public static void LineStipple(int factor, ushort pattern) => _glLineStipple(factor, pattern);
        public static void LineWidth(float width) => _glLineWidth(width);
        public static void LinkProgram(uint program) => _glLinkProgram(program);
        public static void ListBase(uint @base) => _glListBase(@base);
        public static void LoadIdentity() => _glLoadIdentity();
        public static void LoadMatrixd(double* m) => _glLoadMatrixd(m);
        public static void LoadMatrixf(float* m) => _glLoadMatrixf(m);
        public static void LoadName(uint name) => _glLoadName(name);
        public static void LoadTransposeMatrixd(double* m) => _glLoadTransposeMatrixd(m);
        public static void LoadTransposeMatrixf(float* m) => _glLoadTransposeMatrixf(m);
        public static void LogicOp(SpiceEngine.GLFWBindings.GLEnums.LogicOp opcode) => _glLogicOp(opcode);
        public static void Map1d(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, double u1, double u2, int stride, int order, double* points) => _glMap1d(target, u1, u2, stride, order, points);
        public static void Map1f(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, float u1, float u2, int stride, int order, float* points) => _glMap1f(target, u1, u2, stride, order, points);
        public static void Map2d(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, double u1, double u2, int ustride, int uorder, double v1, double v2, int vstride, int vorder, double* points) => _glMap2d(target, u1, u2, ustride, uorder, v1, v2, vstride, vorder, points);
        public static void Map2f(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, float* points) => _glMap2f(target, u1, u2, ustride, uorder, v1, v2, vstride, vorder, points);
        public static void* MapBufferInternal(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, SpiceEngine.GLFWBindings.GLEnums.BufferAccessARB access) => _glMapBuffer(target, access);
        public static void* MapBufferRangeInternal(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, IntPtr offset, IntPtr length, SpiceEngine.GLFWBindings.GLEnums.MapBufferAccessMask access) => _glMapBufferRange(target, offset, length, access);
        public static void MapGrid1d(int un, double u1, double u2) => _glMapGrid1d(un, u1, u2);
        public static void MapGrid1f(int un, float u1, float u2) => _glMapGrid1f(un, u1, u2);
        public static void MapGrid2d(int un, double u1, double u2, int vn, double v1, double v2) => _glMapGrid2d(un, u1, u2, vn, v1, v2);
        public static void MapGrid2f(int un, float u1, float u2, int vn, float v1, float v2) => _glMapGrid2f(un, u1, u2, vn, v1, v2);
        public static void* MapNamedBuffer(uint buffer, SpiceEngine.GLFWBindings.GLEnums.BufferAccessARB access) => _glMapNamedBuffer(buffer, access);
        public static void* MapNamedBufferRange(uint buffer, IntPtr offset, IntPtr length, SpiceEngine.GLFWBindings.GLEnums.MapBufferAccessMask access) => _glMapNamedBufferRange(buffer, offset, length, access);
        public static void Materialf(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, float param) => _glMaterialf(face, pname, param);
        public static void Materialfv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, float* @params) => _glMaterialfv(face, pname, @params);
        public static void Materiali(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, int param) => _glMateriali(face, pname, param);
        public static void Materialiv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, int* @params) => _glMaterialiv(face, pname, @params);
        public static void MatrixMode(SpiceEngine.GLFWBindings.GLEnums.MatrixMode mode) => _glMatrixMode(mode);
        public static void MemoryBarrier(SpiceEngine.GLFWBindings.GLEnums.MemoryBarrierMask barriers) => _glMemoryBarrier(barriers);
        public static void MemoryBarrierByRegion(SpiceEngine.GLFWBindings.GLEnums.MemoryBarrierMask barriers) => _glMemoryBarrierByRegion(barriers);
        public static void MinSampleShading(float value) => _glMinSampleShading(value);
        public static void MultiDrawArrays(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int* first, int* count, int drawcount) => _glMultiDrawArrays(mode, first, count, drawcount);
        public static void MultiDrawArraysIndirect(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, void* indirect, int drawcount, int stride) => _glMultiDrawArraysIndirect(mode, indirect, drawcount, stride);
        public static void MultiDrawArraysIndirectCount(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, void* indirect, IntPtr drawcount, int maxdrawcount, int stride) => _glMultiDrawArraysIndirectCount(mode, indirect, drawcount, maxdrawcount, stride);
        public static void MultiDrawElements(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int* count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void** indices, int drawcount) => _glMultiDrawElements(mode, count, type, indices, drawcount);
        public static void MultiDrawElementsBaseVertex(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int* count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void** indices, int drawcount, int* basevertex) => _glMultiDrawElementsBaseVertex(mode, count, type, indices, drawcount, basevertex);
        public static void MultiDrawElementsIndirect(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indirect, int drawcount, int stride) => _glMultiDrawElementsIndirect(mode, type, indirect, drawcount, stride);
        public static void MultiDrawElementsIndirectCount(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void* indirect, IntPtr drawcount, int maxdrawcount, int stride) => _glMultiDrawElementsIndirectCount(mode, type, indirect, drawcount, maxdrawcount, stride);
        public static void MultiTexCoord1d(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, double s) => _glMultiTexCoord1d(target, s);
        public static void MultiTexCoord1dv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, double* v) => _glMultiTexCoord1dv(target, v);
        public static void MultiTexCoord1f(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float s) => _glMultiTexCoord1f(target, s);
        public static void MultiTexCoord1fv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float* v) => _glMultiTexCoord1fv(target, v);
        public static void MultiTexCoord1i(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int s) => _glMultiTexCoord1i(target, s);
        public static void MultiTexCoord1iv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int* v) => _glMultiTexCoord1iv(target, v);
        public static void MultiTexCoord1s(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, short s) => _glMultiTexCoord1s(target, s);
        public static void MultiTexCoord1sv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, short* v) => _glMultiTexCoord1sv(target, v);
        public static void MultiTexCoord2d(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, double s, double t) => _glMultiTexCoord2d(target, s, t);
        public static void MultiTexCoord2dv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, double* v) => _glMultiTexCoord2dv(target, v);
        public static void MultiTexCoord2f(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float s, float t) => _glMultiTexCoord2f(target, s, t);
        public static void MultiTexCoord2fv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float* v) => _glMultiTexCoord2fv(target, v);
        public static void MultiTexCoord2i(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int s, int t) => _glMultiTexCoord2i(target, s, t);
        public static void MultiTexCoord2iv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int* v) => _glMultiTexCoord2iv(target, v);
        public static void MultiTexCoord2s(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, short s, short t) => _glMultiTexCoord2s(target, s, t);
        public static void MultiTexCoord2sv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, short* v) => _glMultiTexCoord2sv(target, v);
        public static void MultiTexCoord3d(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, double s, double t, double r) => _glMultiTexCoord3d(target, s, t, r);
        public static void MultiTexCoord3dv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, double* v) => _glMultiTexCoord3dv(target, v);
        public static void MultiTexCoord3f(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float s, float t, float r) => _glMultiTexCoord3f(target, s, t, r);
        public static void MultiTexCoord3fv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float* v) => _glMultiTexCoord3fv(target, v);
        public static void MultiTexCoord3i(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int s, int t, int r) => _glMultiTexCoord3i(target, s, t, r);
        public static void MultiTexCoord3iv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int* v) => _glMultiTexCoord3iv(target, v);
        public static void MultiTexCoord3s(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, short s, short t, short r) => _glMultiTexCoord3s(target, s, t, r);
        public static void MultiTexCoord3sv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, short* v) => _glMultiTexCoord3sv(target, v);
        public static void MultiTexCoord4d(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, double s, double t, double r, double q) => _glMultiTexCoord4d(target, s, t, r, q);
        public static void MultiTexCoord4dv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, double* v) => _glMultiTexCoord4dv(target, v);
        public static void MultiTexCoord4f(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float s, float t, float r, float q) => _glMultiTexCoord4f(target, s, t, r, q);
        public static void MultiTexCoord4fv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float* v) => _glMultiTexCoord4fv(target, v);
        public static void MultiTexCoord4i(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int s, int t, int r, int q) => _glMultiTexCoord4i(target, s, t, r, q);
        public static void MultiTexCoord4iv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int* v) => _glMultiTexCoord4iv(target, v);
        public static void MultiTexCoord4s(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, short s, short t, short r, short q) => _glMultiTexCoord4s(target, s, t, r, q);
        public static void MultiTexCoord4sv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, short* v) => _glMultiTexCoord4sv(target, v);
        public static void MultiTexCoordP1ui(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint coords) => _glMultiTexCoordP1ui(texture, type, coords);
        public static void MultiTexCoordP1uiv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint* coords) => _glMultiTexCoordP1uiv(texture, type, coords);
        public static void MultiTexCoordP2ui(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint coords) => _glMultiTexCoordP2ui(texture, type, coords);
        public static void MultiTexCoordP2uiv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint* coords) => _glMultiTexCoordP2uiv(texture, type, coords);
        public static void MultiTexCoordP3ui(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint coords) => _glMultiTexCoordP3ui(texture, type, coords);
        public static void MultiTexCoordP3uiv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint* coords) => _glMultiTexCoordP3uiv(texture, type, coords);
        public static void MultiTexCoordP4ui(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint coords) => _glMultiTexCoordP4ui(texture, type, coords);
        public static void MultiTexCoordP4uiv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint* coords) => _glMultiTexCoordP4uiv(texture, type, coords);
        public static void MultMatrixd(double* m) => _glMultMatrixd(m);
        public static void MultMatrixf(float* m) => _glMultMatrixf(m);
        public static void MultTransposeMatrixd(double* m) => _glMultTransposeMatrixd(m);
        public static void MultTransposeMatrixf(float* m) => _glMultTransposeMatrixf(m);
        public static void NamedBufferData(uint buffer, IntPtr size, void* data, SpiceEngine.GLFWBindings.GLEnums.VertexBufferObjectUsage usage) => _glNamedBufferData(buffer, size, data, usage);
        public static void NamedBufferStorage(uint buffer, IntPtr size, void* data, SpiceEngine.GLFWBindings.GLEnums.BufferStorageMask flags) => _glNamedBufferStorage(buffer, size, data, flags);
        public static void NamedBufferSubData(uint buffer, IntPtr offset, IntPtr size, void* data) => _glNamedBufferSubData(buffer, offset, size, data);
        public static void NamedFramebufferDrawBuffer(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.ColorBuffer buf) => _glNamedFramebufferDrawBuffer(framebuffer, buf);
        public static void NamedFramebufferDrawBuffers(uint framebuffer, int n, SpiceEngine.GLFWBindings.GLEnums.ColorBuffer* bufs) => _glNamedFramebufferDrawBuffers(framebuffer, n, bufs);
        public static void NamedFramebufferParameteri(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferParameterName pname, int param) => _glNamedFramebufferParameteri(framebuffer, pname, param);
        public static void NamedFramebufferReadBuffer(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.ColorBuffer src) => _glNamedFramebufferReadBuffer(framebuffer, src);
        public static void NamedFramebufferRenderbuffer(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget renderbuffertarget, uint renderbuffer) => _glNamedFramebufferRenderbuffer(framebuffer, attachment, renderbuffertarget, renderbuffer);
        public static void NamedFramebufferTexture(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, uint texture, int level) => _glNamedFramebufferTexture(framebuffer, attachment, texture, level);
        public static void NamedFramebufferTextureLayer(uint framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, uint texture, int level, int layer) => _glNamedFramebufferTextureLayer(framebuffer, attachment, texture, level, layer);
        public static void NamedRenderbufferStorage(uint renderbuffer, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height) => _glNamedRenderbufferStorage(renderbuffer, internalformat, width, height);
        public static void NamedRenderbufferStorageMultisample(uint renderbuffer, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height) => _glNamedRenderbufferStorageMultisample(renderbuffer, samples, internalformat, width, height);
        public static void NewList(uint list, SpiceEngine.GLFWBindings.GLEnums.ListMode mode) => _glNewList(list, mode);
        public static void Normal3b(byte nx, byte ny, byte nz) => _glNormal3b(nx, ny, nz);
        public static void Normal3bv(byte* v) => _glNormal3bv(v);
        public static void Normal3d(double nx, double ny, double nz) => _glNormal3d(nx, ny, nz);
        public static void Normal3dv(double* v) => _glNormal3dv(v);
        public static void Normal3f(float nx, float ny, float nz) => _glNormal3f(nx, ny, nz);
        public static void Normal3fv(float* v) => _glNormal3fv(v);
        public static void Normal3i(int nx, int ny, int nz) => _glNormal3i(nx, ny, nz);
        public static void Normal3iv(int* v) => _glNormal3iv(v);
        public static void Normal3s(short nx, short ny, short nz) => _glNormal3s(nx, ny, nz);
        public static void Normal3sv(short* v) => _glNormal3sv(v);
        public static void NormalP3ui(SpiceEngine.GLFWBindings.GLEnums.NormalPointerType type, uint coords) => _glNormalP3ui(type, coords);
        public static void NormalP3uiv(SpiceEngine.GLFWBindings.GLEnums.NormalPointerType type, uint* coords) => _glNormalP3uiv(type, coords);
        public static void NormalPointer(SpiceEngine.GLFWBindings.GLEnums.NormalPointerType type, int stride, void* pointer) => _glNormalPointer(type, stride, pointer);
        public static void ObjectLabel(SpiceEngine.GLFWBindings.GLEnums.ObjectIdentifier identifier, uint name, int length, char* label) => _glObjectLabel(identifier, name, length, label);
        public static void ObjectPtrLabel(void* ptr, int length, char* label) => _glObjectPtrLabel(ptr, length, label);
        public static void Ortho(double left, double right, double bottom, double top, double zNear, double zFar) => _glOrtho(left, right, bottom, top, zNear, zFar);
        public static void PassThrough(float token) => _glPassThrough(token);
        public static void PatchParameterfv(SpiceEngine.GLFWBindings.GLEnums.PatchParameterName pname, float* values) => _glPatchParameterfv(pname, values);
        public static void PatchParameteri(SpiceEngine.GLFWBindings.GLEnums.PatchParameterName pname, int value) => _glPatchParameteri(pname, value);
        public static void PauseTransformFeedback() => _glPauseTransformFeedback();
        public static void PixelMapfv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int mapsize, float* values) => _glPixelMapfv(map, mapsize, values);
        public static void PixelMapuiv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int mapsize, uint* values) => _glPixelMapuiv(map, mapsize, values);
        public static void PixelMapusv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int mapsize, ushort* values) => _glPixelMapusv(map, mapsize, values);
        public static void PixelStoref(SpiceEngine.GLFWBindings.GLEnums.PixelStoreParameter pname, float param) => _glPixelStoref(pname, param);
        public static void PixelStorei(SpiceEngine.GLFWBindings.GLEnums.PixelStoreParameter pname, int param) => _glPixelStorei(pname, param);
        public static void PixelTransferf(SpiceEngine.GLFWBindings.GLEnums.PixelTransferParameter pname, float param) => _glPixelTransferf(pname, param);
        public static void PixelTransferi(SpiceEngine.GLFWBindings.GLEnums.PixelTransferParameter pname, int param) => _glPixelTransferi(pname, param);
        public static void PixelZoom(float xfactor, float yfactor) => _glPixelZoom(xfactor, yfactor);
        public static void PointParameterf(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB pname, float param) => _glPointParameterf(pname, param);
        public static void PointParameterfv(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB pname, float* @params) => _glPointParameterfv(pname, @params);
        public static void PointParameteri(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB pname, int param) => _glPointParameteri(pname, param);
        public static void PointParameteriv(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB pname, int* @params) => _glPointParameteriv(pname, @params);
        public static void PointSize(float size) => _glPointSize(size);
        public static void PolygonMode(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.PolygonMode mode) => _glPolygonMode(face, mode);
        public static void PolygonOffset(float factor, float units) => _glPolygonOffset(factor, units);
        public static void PolygonOffsetClamp(float factor, float units, float clamp) => _glPolygonOffsetClamp(factor, units, clamp);
        public static void PolygonStipple(byte* mask) => _glPolygonStipple(mask);
        public static void PopAttrib() => _glPopAttrib();
        public static void PopClientAttrib() => _glPopClientAttrib();
        public static void PopDebugGroup() => _glPopDebugGroup();
        public static void PopMatrix() => _glPopMatrix();
        public static void PopName() => _glPopName();
        public static void PrimitiveRestartIndex(uint index) => _glPrimitiveRestartIndex(index);
        public static void PrioritizeTextures(int n, uint* textures, float* priorities) => _glPrioritizeTextures(n, textures, priorities);
        public static void ProgramBinary(uint program, int binaryFormat, void* binary, int length) => _glProgramBinary(program, binaryFormat, binary, length);
        public static void ProgramParameteri(uint program, SpiceEngine.GLFWBindings.GLEnums.ProgramParameterPName pname, int value) => _glProgramParameteri(program, pname, value);
        public static void ProgramUniform1d(uint program, int location, double v0) => _glProgramUniform1d(program, location, v0);
        public static void ProgramUniform1dv(uint program, int location, int count, double* value) => _glProgramUniform1dv(program, location, count, value);
        public static void ProgramUniform1f(uint program, int location, float v0) => _glProgramUniform1f(program, location, v0);
        public static void ProgramUniform1fv(uint program, int location, int count, float* value) => _glProgramUniform1fv(program, location, count, value);
        public static void ProgramUniform1i(uint program, int location, int v0) => _glProgramUniform1i(program, location, v0);
        public static void ProgramUniform1iv(uint program, int location, int count, int* value) => _glProgramUniform1iv(program, location, count, value);
        public static void ProgramUniform1ui(uint program, int location, uint v0) => _glProgramUniform1ui(program, location, v0);
        public static void ProgramUniform1uiv(uint program, int location, int count, uint* value) => _glProgramUniform1uiv(program, location, count, value);
        public static void ProgramUniform2d(uint program, int location, double v0, double v1) => _glProgramUniform2d(program, location, v0, v1);
        public static void ProgramUniform2dv(uint program, int location, int count, double* value) => _glProgramUniform2dv(program, location, count, value);
        public static void ProgramUniform2f(uint program, int location, float v0, float v1) => _glProgramUniform2f(program, location, v0, v1);
        public static void ProgramUniform2fv(uint program, int location, int count, float* value) => _glProgramUniform2fv(program, location, count, value);
        public static void ProgramUniform2i(uint program, int location, int v0, int v1) => _glProgramUniform2i(program, location, v0, v1);
        public static void ProgramUniform2iv(uint program, int location, int count, int* value) => _glProgramUniform2iv(program, location, count, value);
        public static void ProgramUniform2ui(uint program, int location, uint v0, uint v1) => _glProgramUniform2ui(program, location, v0, v1);
        public static void ProgramUniform2uiv(uint program, int location, int count, uint* value) => _glProgramUniform2uiv(program, location, count, value);
        public static void ProgramUniform3d(uint program, int location, double v0, double v1, double v2) => _glProgramUniform3d(program, location, v0, v1, v2);
        public static void ProgramUniform3dv(uint program, int location, int count, double* value) => _glProgramUniform3dv(program, location, count, value);
        public static void ProgramUniform3f(uint program, int location, float v0, float v1, float v2) => _glProgramUniform3f(program, location, v0, v1, v2);
        public static void ProgramUniform3fv(uint program, int location, int count, float* value) => _glProgramUniform3fv(program, location, count, value);
        public static void ProgramUniform3i(uint program, int location, int v0, int v1, int v2) => _glProgramUniform3i(program, location, v0, v1, v2);
        public static void ProgramUniform3iv(uint program, int location, int count, int* value) => _glProgramUniform3iv(program, location, count, value);
        public static void ProgramUniform3ui(uint program, int location, uint v0, uint v1, uint v2) => _glProgramUniform3ui(program, location, v0, v1, v2);
        public static void ProgramUniform3uiv(uint program, int location, int count, uint* value) => _glProgramUniform3uiv(program, location, count, value);
        public static void ProgramUniform4d(uint program, int location, double v0, double v1, double v2, double v3) => _glProgramUniform4d(program, location, v0, v1, v2, v3);
        public static void ProgramUniform4dv(uint program, int location, int count, double* value) => _glProgramUniform4dv(program, location, count, value);
        public static void ProgramUniform4f(uint program, int location, float v0, float v1, float v2, float v3) => _glProgramUniform4f(program, location, v0, v1, v2, v3);
        public static void ProgramUniform4fv(uint program, int location, int count, float* value) => _glProgramUniform4fv(program, location, count, value);
        public static void ProgramUniform4i(uint program, int location, int v0, int v1, int v2, int v3) => _glProgramUniform4i(program, location, v0, v1, v2, v3);
        public static void ProgramUniform4iv(uint program, int location, int count, int* value) => _glProgramUniform4iv(program, location, count, value);
        public static void ProgramUniform4ui(uint program, int location, uint v0, uint v1, uint v2, uint v3) => _glProgramUniform4ui(program, location, v0, v1, v2, v3);
        public static void ProgramUniform4uiv(uint program, int location, int count, uint* value) => _glProgramUniform4uiv(program, location, count, value);
        public static void ProgramUniformMatrix2dv(uint program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix2dv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix2fv(uint program, int location, int count, bool transpose, float* value) => _glProgramUniformMatrix2fv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix2x3dv(uint program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix2x3dv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix2x3fv(uint program, int location, int count, bool transpose, float* value) => _glProgramUniformMatrix2x3fv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix2x4dv(uint program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix2x4dv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix2x4fv(uint program, int location, int count, bool transpose, float* value) => _glProgramUniformMatrix2x4fv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix3dv(uint program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix3dv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix3fv(uint program, int location, int count, bool transpose, float* value) => _glProgramUniformMatrix3fv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix3x2dv(uint program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix3x2dv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix3x2fv(uint program, int location, int count, bool transpose, float* value) => _glProgramUniformMatrix3x2fv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix3x4dv(uint program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix3x4dv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix3x4fv(uint program, int location, int count, bool transpose, float* value) => _glProgramUniformMatrix3x4fv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix4dv(uint program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix4dv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix4fv(uint program, int location, int count, bool transpose, float* value) => _glProgramUniformMatrix4fv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix4x2dv(uint program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix4x2dv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix4x2fv(uint program, int location, int count, bool transpose, float* value) => _glProgramUniformMatrix4x2fv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix4x3dv(uint program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix4x3dv(program, location, count, transpose, value);
        public static void ProgramUniformMatrix4x3fv(uint program, int location, int count, bool transpose, float* value) => _glProgramUniformMatrix4x3fv(program, location, count, transpose, value);
        public static void ProvokingVertex(SpiceEngine.GLFWBindings.GLEnums.VertexProvokingMode mode) => _glProvokingVertex(mode);
        public static void PushAttrib(SpiceEngine.GLFWBindings.GLEnums.AttribMask mask) => _glPushAttrib(mask);
        public static void PushClientAttrib(SpiceEngine.GLFWBindings.GLEnums.ClientAttribMask mask) => _glPushClientAttrib(mask);
        public static void PushDebugGroup(SpiceEngine.GLFWBindings.GLEnums.DebugSource source, uint id, int length, char* message) => _glPushDebugGroup(source, id, length, message);
        public static void PushMatrix() => _glPushMatrix();
        public static void PushName(uint name) => _glPushName(name);
        public static void QueryCounter(uint id, SpiceEngine.GLFWBindings.GLEnums.QueryCounterTarget target) => _glQueryCounter(id, target);
        public static void RasterPos2d(double x, double y) => _glRasterPos2d(x, y);
        public static void RasterPos2dv(double* v) => _glRasterPos2dv(v);
        public static void RasterPos2f(float x, float y) => _glRasterPos2f(x, y);
        public static void RasterPos2fv(float* v) => _glRasterPos2fv(v);
        public static void RasterPos2i(int x, int y) => _glRasterPos2i(x, y);
        public static void RasterPos2iv(int* v) => _glRasterPos2iv(v);
        public static void RasterPos2s(short x, short y) => _glRasterPos2s(x, y);
        public static void RasterPos2sv(short* v) => _glRasterPos2sv(v);
        public static void RasterPos3d(double x, double y, double z) => _glRasterPos3d(x, y, z);
        public static void RasterPos3dv(double* v) => _glRasterPos3dv(v);
        public static void RasterPos3f(float x, float y, float z) => _glRasterPos3f(x, y, z);
        public static void RasterPos3fv(float* v) => _glRasterPos3fv(v);
        public static void RasterPos3i(int x, int y, int z) => _glRasterPos3i(x, y, z);
        public static void RasterPos3iv(int* v) => _glRasterPos3iv(v);
        public static void RasterPos3s(short x, short y, short z) => _glRasterPos3s(x, y, z);
        public static void RasterPos3sv(short* v) => _glRasterPos3sv(v);
        public static void RasterPos4d(double x, double y, double z, double w) => _glRasterPos4d(x, y, z, w);
        public static void RasterPos4dv(double* v) => _glRasterPos4dv(v);
        public static void RasterPos4f(float x, float y, float z, float w) => _glRasterPos4f(x, y, z, w);
        public static void RasterPos4fv(float* v) => _glRasterPos4fv(v);
        public static void RasterPos4i(int x, int y, int z, int w) => _glRasterPos4i(x, y, z, w);
        public static void RasterPos4iv(int* v) => _glRasterPos4iv(v);
        public static void RasterPos4s(short x, short y, short z, short w) => _glRasterPos4s(x, y, z, w);
        public static void RasterPos4sv(short* v) => _glRasterPos4sv(v);
        public static void ReadBuffer(SpiceEngine.GLFWBindings.GLEnums.ReadBufferMode src) => _glReadBuffer(src);
        public static void ReadnPixels(int x, int y, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, void* data) => _glReadnPixels(x, y, width, height, format, type, bufSize, data);
        public static void ReadPixels(int x, int y, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glReadPixels(x, y, width, height, format, type, pixels);
        public static void Rectd(double x1, double y1, double x2, double y2) => _glRectd(x1, y1, x2, y2);
        public static void Rectdv(double* v1, double* v2) => _glRectdv(v1, v2);
        public static void Rectf(float x1, float y1, float x2, float y2) => _glRectf(x1, y1, x2, y2);
        public static void Rectfv(float* v1, float* v2) => _glRectfv(v1, v2);
        public static void Recti(int x1, int y1, int x2, int y2) => _glRecti(x1, y1, x2, y2);
        public static void Rectiv(int* v1, int* v2) => _glRectiv(v1, v2);
        public static void Rects(short x1, short y1, short x2, short y2) => _glRects(x1, y1, x2, y2);
        public static void Rectsv(short* v1, short* v2) => _glRectsv(v1, v2);
        public static void ReleaseShaderCompiler() => _glReleaseShaderCompiler();
        public static void RenderbufferStorage(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height) => _glRenderbufferStorage(target, internalformat, width, height);
        public static void RenderbufferStorageMultisample(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget target, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height) => _glRenderbufferStorageMultisample(target, samples, internalformat, width, height);
        public static int RenderMode(SpiceEngine.GLFWBindings.GLEnums.RenderingMode mode) => _glRenderMode(mode);
        public static void ResumeTransformFeedback() => _glResumeTransformFeedback();
        public static void Rotated(double angle, double x, double y, double z) => _glRotated(angle, x, y, z);
        public static void Rotatef(float angle, float x, float y, float z) => _glRotatef(angle, x, y, z);
        public static void SampleCoverage(float value, bool invert) => _glSampleCoverage(value, invert);
        public static void SampleMaski(uint maskNumber, int mask) => _glSampleMaski(maskNumber, mask);
        public static void SamplerParameterf(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterF pname, float param) => _glSamplerParameterf(sampler, pname, param);
        public static void SamplerParameterfv(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterF pname, float* param) => _glSamplerParameterfv(sampler, pname, param);
        public static void SamplerParameteri(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int param) => _glSamplerParameteri(sampler, pname, param);
        public static void SamplerParameterIiv(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int* param) => _glSamplerParameterIiv(sampler, pname, param);
        public static void SamplerParameterIuiv(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, uint* param) => _glSamplerParameterIuiv(sampler, pname, param);
        public static void SamplerParameteriv(uint sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int* param) => _glSamplerParameteriv(sampler, pname, param);
        public static void Scaled(double x, double y, double z) => _glScaled(x, y, z);
        public static void Scalef(float x, float y, float z) => _glScalef(x, y, z);
        public static void Scissor(int x, int y, int width, int height) => _glScissor(x, y, width, height);
        public static void ScissorArrayv(uint first, int count, int* v) => _glScissorArrayv(first, count, v);
        public static void ScissorIndexed(uint index, int left, int bottom, int width, int height) => _glScissorIndexed(index, left, bottom, width, height);
        public static void ScissorIndexedv(uint index, int* v) => _glScissorIndexedv(index, v);
        public static void SecondaryColor3b(byte red, byte green, byte blue) => _glSecondaryColor3b(red, green, blue);
        public static void SecondaryColor3bv(byte* v) => _glSecondaryColor3bv(v);
        public static void SecondaryColor3d(double red, double green, double blue) => _glSecondaryColor3d(red, green, blue);
        public static void SecondaryColor3dv(double* v) => _glSecondaryColor3dv(v);
        public static void SecondaryColor3f(float red, float green, float blue) => _glSecondaryColor3f(red, green, blue);
        public static void SecondaryColor3fv(float* v) => _glSecondaryColor3fv(v);
        public static void SecondaryColor3i(int red, int green, int blue) => _glSecondaryColor3i(red, green, blue);
        public static void SecondaryColor3iv(int* v) => _glSecondaryColor3iv(v);
        public static void SecondaryColor3s(short red, short green, short blue) => _glSecondaryColor3s(red, green, blue);
        public static void SecondaryColor3sv(short* v) => _glSecondaryColor3sv(v);
        public static void SecondaryColor3ub(byte red, byte green, byte blue) => _glSecondaryColor3ub(red, green, blue);
        public static void SecondaryColor3ubv(byte* v) => _glSecondaryColor3ubv(v);
        public static void SecondaryColor3ui(uint red, uint green, uint blue) => _glSecondaryColor3ui(red, green, blue);
        public static void SecondaryColor3uiv(uint* v) => _glSecondaryColor3uiv(v);
        public static void SecondaryColor3us(ushort red, ushort green, ushort blue) => _glSecondaryColor3us(red, green, blue);
        public static void SecondaryColor3usv(ushort* v) => _glSecondaryColor3usv(v);
        public static void SecondaryColorP3ui(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, uint color) => _glSecondaryColorP3ui(type, color);
        public static void SecondaryColorP3uiv(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, uint* color) => _glSecondaryColorP3uiv(type, color);
        public static void SecondaryColorPointer(int size, SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int stride, void* pointer) => _glSecondaryColorPointer(size, type, stride, pointer);
        public static void SelectBuffer(int size, uint* buffer) => _glSelectBuffer(size, buffer);
        public static void ShadeModel(SpiceEngine.GLFWBindings.GLEnums.ShadingModel mode) => _glShadeModel(mode);
        public static void ShaderBinary(int count, uint* shaders, SpiceEngine.GLFWBindings.GLEnums.ShaderBinaryFormat binaryFormat, void* binary, int length) => _glShaderBinary(count, shaders, binaryFormat, binary, length);
        public static void ShaderSource(uint shader, int count, char** @string, int* length) => _glShaderSource(shader, count, @string, length);
        public static void ShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding) => _glShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding);
        public static void SpecializeShader(uint shader, char* pEntryPoint, uint numSpecializationConstants, uint* pConstantIndex, uint* pConstantValue) => _glSpecializeShader(shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue);
        public static void StencilFunc(SpiceEngine.GLFWBindings.GLEnums.StencilFunction func, int @ref, uint mask) => _glStencilFunc(func, @ref, mask);
        public static void StencilFuncSeparate(SpiceEngine.GLFWBindings.GLEnums.StencilFaceDirection face, SpiceEngine.GLFWBindings.GLEnums.StencilFunction func, int @ref, uint mask) => _glStencilFuncSeparate(face, func, @ref, mask);
        public static void StencilMask(uint mask) => _glStencilMask(mask);
        public static void StencilMaskSeparate(SpiceEngine.GLFWBindings.GLEnums.StencilFaceDirection face, uint mask) => _glStencilMaskSeparate(face, mask);
        public static void StencilOp(SpiceEngine.GLFWBindings.GLEnums.StencilOp fail, SpiceEngine.GLFWBindings.GLEnums.StencilOp zfail, SpiceEngine.GLFWBindings.GLEnums.StencilOp zpass) => _glStencilOp(fail, zfail, zpass);
        public static void StencilOpSeparate(SpiceEngine.GLFWBindings.GLEnums.StencilFaceDirection face, SpiceEngine.GLFWBindings.GLEnums.StencilOp sfail, SpiceEngine.GLFWBindings.GLEnums.StencilOp dpfail, SpiceEngine.GLFWBindings.GLEnums.StencilOp dppass) => _glStencilOpSeparate(face, sfail, dpfail, dppass);
        public static void TexBuffer(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, uint buffer) => _glTexBuffer(target, internalformat, buffer);
        public static void TexBufferRange(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, uint buffer, IntPtr offset, IntPtr size) => _glTexBufferRange(target, internalformat, buffer, offset, size);
        public static void TexCoord1d(double s) => _glTexCoord1d(s);
        public static void TexCoord1dv(double* v) => _glTexCoord1dv(v);
        public static void TexCoord1f(float s) => _glTexCoord1f(s);
        public static void TexCoord1fv(float* v) => _glTexCoord1fv(v);
        public static void TexCoord1i(int s) => _glTexCoord1i(s);
        public static void TexCoord1iv(int* v) => _glTexCoord1iv(v);
        public static void TexCoord1s(short s) => _glTexCoord1s(s);
        public static void TexCoord1sv(short* v) => _glTexCoord1sv(v);
        public static void TexCoord2d(double s, double t) => _glTexCoord2d(s, t);
        public static void TexCoord2dv(double* v) => _glTexCoord2dv(v);
        public static void TexCoord2f(float s, float t) => _glTexCoord2f(s, t);
        public static void TexCoord2fv(float* v) => _glTexCoord2fv(v);
        public static void TexCoord2i(int s, int t) => _glTexCoord2i(s, t);
        public static void TexCoord2iv(int* v) => _glTexCoord2iv(v);
        public static void TexCoord2s(short s, short t) => _glTexCoord2s(s, t);
        public static void TexCoord2sv(short* v) => _glTexCoord2sv(v);
        public static void TexCoord3d(double s, double t, double r) => _glTexCoord3d(s, t, r);
        public static void TexCoord3dv(double* v) => _glTexCoord3dv(v);
        public static void TexCoord3f(float s, float t, float r) => _glTexCoord3f(s, t, r);
        public static void TexCoord3fv(float* v) => _glTexCoord3fv(v);
        public static void TexCoord3i(int s, int t, int r) => _glTexCoord3i(s, t, r);
        public static void TexCoord3iv(int* v) => _glTexCoord3iv(v);
        public static void TexCoord3s(short s, short t, short r) => _glTexCoord3s(s, t, r);
        public static void TexCoord3sv(short* v) => _glTexCoord3sv(v);
        public static void TexCoord4d(double s, double t, double r, double q) => _glTexCoord4d(s, t, r, q);
        public static void TexCoord4dv(double* v) => _glTexCoord4dv(v);
        public static void TexCoord4f(float s, float t, float r, float q) => _glTexCoord4f(s, t, r, q);
        public static void TexCoord4fv(float* v) => _glTexCoord4fv(v);
        public static void TexCoord4i(int s, int t, int r, int q) => _glTexCoord4i(s, t, r, q);
        public static void TexCoord4iv(int* v) => _glTexCoord4iv(v);
        public static void TexCoord4s(short s, short t, short r, short q) => _glTexCoord4s(s, t, r, q);
        public static void TexCoord4sv(short* v) => _glTexCoord4sv(v);
        public static void TexCoordP1ui(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint coords) => _glTexCoordP1ui(type, coords);
        public static void TexCoordP1uiv(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint* coords) => _glTexCoordP1uiv(type, coords);
        public static void TexCoordP2ui(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint coords) => _glTexCoordP2ui(type, coords);
        public static void TexCoordP2uiv(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint* coords) => _glTexCoordP2uiv(type, coords);
        public static void TexCoordP3ui(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint coords) => _glTexCoordP3ui(type, coords);
        public static void TexCoordP3uiv(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint* coords) => _glTexCoordP3uiv(type, coords);
        public static void TexCoordP4ui(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint coords) => _glTexCoordP4ui(type, coords);
        public static void TexCoordP4uiv(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, uint* coords) => _glTexCoordP4uiv(type, coords);
        public static void TexCoordPointer(int size, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int stride, void* pointer) => _glTexCoordPointer(size, type, stride, pointer);
        public static void TexEnvf(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, float param) => _glTexEnvf(target, pname, param);
        public static void TexEnvfv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, float* @params) => _glTexEnvfv(target, pname, @params);
        public static void TexEnvi(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, int param) => _glTexEnvi(target, pname, param);
        public static void TexEnviv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, int* @params) => _glTexEnviv(target, pname, @params);
        public static void TexGend(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, double param) => _glTexGend(coord, pname, param);
        public static void TexGendv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, double* @params) => _glTexGendv(coord, pname, @params);
        public static void TexGenf(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, float param) => _glTexGenf(coord, pname, param);
        public static void TexGenfv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, float* @params) => _glTexGenfv(coord, pname, @params);
        public static void TexGeni(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, int param) => _glTexGeni(coord, pname, param);
        public static void TexGeniv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, int* @params) => _glTexGeniv(coord, pname, @params);
        public static void TexImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int border, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glTexImage1D(target, level, internalformat, width, border, format, type, pixels);
        public static void TexImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int border, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);
        public static void TexImage2DMultisample(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, bool fixedsamplelocations) => _glTexImage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);
        public static void TexImage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth, int border, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glTexImage3D(target, level, internalformat, width, height, depth, border, format, type, pixels);
        public static void TexImage3DMultisample(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTexImage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);
        public static void TexParameterf(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, float param) => _glTexParameterf(target, pname, param);
        public static void TexParameterfv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, float* @params) => _glTexParameterfv(target, pname, @params);
        public static void TexParameteri(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int param) => _glTexParameteri(target, pname, param);
        public static void TexParameterIiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int* @params) => _glTexParameterIiv(target, pname, @params);
        public static void TexParameterIuiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, uint* @params) => _glTexParameterIuiv(target, pname, @params);
        public static void TexParameteriv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int* @params) => _glTexParameteriv(target, pname, @params);
        public static void TexStorage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int levels, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width) => _glTexStorage1D(target, levels, internalformat, width);
        public static void TexStorage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int levels, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height) => _glTexStorage2D(target, levels, internalformat, width, height);
        public static void TexStorage2DMultisample(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, bool fixedsamplelocations) => _glTexStorage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);
        public static void TexStorage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int levels, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth) => _glTexStorage3D(target, levels, internalformat, width, height, depth);
        public static void TexStorage3DMultisample(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTexStorage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);
        public static void TexSubImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int width, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glTexSubImage1D(target, level, xoffset, width, format, type, pixels);
        public static void TexSubImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);
        public static void TexSubImage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);
        public static void TextureBarrier() => _glTextureBarrier();
        public static void TextureBuffer(uint texture, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, uint buffer) => _glTextureBuffer(texture, internalformat, buffer);
        public static void TextureBufferRange(uint texture, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, uint buffer, IntPtr offset, IntPtr size) => _glTextureBufferRange(texture, internalformat, buffer, offset, size);
        public static void TextureParameterf(uint texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, float param) => _glTextureParameterf(texture, pname, param);
        public static void TextureParameterfv(uint texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, float* param) => _glTextureParameterfv(texture, pname, param);
        public static void TextureParameteri(uint texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int param) => _glTextureParameteri(texture, pname, param);
        public static void TextureParameterIiv(uint texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int* @params) => _glTextureParameterIiv(texture, pname, @params);
        public static void TextureParameterIuiv(uint texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, uint* @params) => _glTextureParameterIuiv(texture, pname, @params);
        public static void TextureParameteriv(uint texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int* param) => _glTextureParameteriv(texture, pname, param);
        public static void TextureStorage1D(uint texture, int levels, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width) => _glTextureStorage1D(texture, levels, internalformat, width);
        public static void TextureStorage2D(uint texture, int levels, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height) => _glTextureStorage2D(texture, levels, internalformat, width, height);
        public static void TextureStorage2DMultisample(uint texture, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, bool fixedsamplelocations) => _glTextureStorage2DMultisample(texture, samples, internalformat, width, height, fixedsamplelocations);
        public static void TextureStorage3D(uint texture, int levels, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth) => _glTextureStorage3D(texture, levels, internalformat, width, height, depth);
        public static void TextureStorage3DMultisample(uint texture, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTextureStorage3DMultisample(texture, samples, internalformat, width, height, depth, fixedsamplelocations);
        public static void TextureSubImage1D(uint texture, int level, int xoffset, int width, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glTextureSubImage1D(texture, level, xoffset, width, format, type, pixels);
        public static void TextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, format, type, pixels);
        public static void TextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, void* pixels) => _glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);
        public static void TextureView(uint texture, SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, uint origtexture, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers) => _glTextureView(texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers);
        public static void TransformFeedbackBufferBase(uint xfb, uint index, uint buffer) => _glTransformFeedbackBufferBase(xfb, index, buffer);
        public static void TransformFeedbackBufferRange(uint xfb, uint index, uint buffer, IntPtr offset, IntPtr size) => _glTransformFeedbackBufferRange(xfb, index, buffer, offset, size);
        public static void TransformFeedbackVaryings(uint program, int count, char** varyings, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackBufferMode bufferMode) => _glTransformFeedbackVaryings(program, count, varyings, bufferMode);
        public static void Translated(double x, double y, double z) => _glTranslated(x, y, z);
        public static void Translatef(float x, float y, float z) => _glTranslatef(x, y, z);
        public static void Uniform1d(int location, double x) => _glUniform1d(location, x);
        public static void Uniform1dv(int location, int count, double* value) => _glUniform1dv(location, count, value);
        public static void Uniform1f(int location, float v0) => _glUniform1f(location, v0);
        public static void Uniform1fv(int location, int count, float* value) => _glUniform1fv(location, count, value);
        public static void Uniform1i(int location, int v0) => _glUniform1i(location, v0);
        public static void Uniform1iv(int location, int count, int* value) => _glUniform1iv(location, count, value);
        public static void Uniform1ui(int location, uint v0) => _glUniform1ui(location, v0);
        public static void Uniform1uiv(int location, int count, uint* value) => _glUniform1uiv(location, count, value);
        public static void Uniform2d(int location, double x, double y) => _glUniform2d(location, x, y);
        public static void Uniform2dv(int location, int count, double* value) => _glUniform2dv(location, count, value);
        public static void Uniform2f(int location, float v0, float v1) => _glUniform2f(location, v0, v1);
        public static void Uniform2fv(int location, int count, float* value) => _glUniform2fv(location, count, value);
        public static void Uniform2i(int location, int v0, int v1) => _glUniform2i(location, v0, v1);
        public static void Uniform2iv(int location, int count, int* value) => _glUniform2iv(location, count, value);
        public static void Uniform2ui(int location, uint v0, uint v1) => _glUniform2ui(location, v0, v1);
        public static void Uniform2uiv(int location, int count, uint* value) => _glUniform2uiv(location, count, value);
        public static void Uniform3d(int location, double x, double y, double z) => _glUniform3d(location, x, y, z);
        public static void Uniform3dv(int location, int count, double* value) => _glUniform3dv(location, count, value);
        public static void Uniform3f(int location, float v0, float v1, float v2) => _glUniform3f(location, v0, v1, v2);
        public static void Uniform3fv(int location, int count, float* value) => _glUniform3fv(location, count, value);
        public static void Uniform3i(int location, int v0, int v1, int v2) => _glUniform3i(location, v0, v1, v2);
        public static void Uniform3iv(int location, int count, int* value) => _glUniform3iv(location, count, value);
        public static void Uniform3ui(int location, uint v0, uint v1, uint v2) => _glUniform3ui(location, v0, v1, v2);
        public static void Uniform3uiv(int location, int count, uint* value) => _glUniform3uiv(location, count, value);
        public static void Uniform4d(int location, double x, double y, double z, double w) => _glUniform4d(location, x, y, z, w);
        public static void Uniform4dv(int location, int count, double* value) => _glUniform4dv(location, count, value);
        public static void Uniform4f(int location, float v0, float v1, float v2, float v3) => _glUniform4f(location, v0, v1, v2, v3);
        public static void Uniform4fv(int location, int count, float* value) => _glUniform4fv(location, count, value);
        public static void Uniform4i(int location, int v0, int v1, int v2, int v3) => _glUniform4i(location, v0, v1, v2, v3);
        public static void Uniform4iv(int location, int count, int* value) => _glUniform4iv(location, count, value);
        public static void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3) => _glUniform4ui(location, v0, v1, v2, v3);
        public static void Uniform4uiv(int location, int count, uint* value) => _glUniform4uiv(location, count, value);
        public static void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) => _glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);
        public static void UniformMatrix2dv(int location, int count, bool transpose, double* value) => _glUniformMatrix2dv(location, count, transpose, value);
        public static void UniformMatrix2fv(int location, int count, bool transpose, float* value) => _glUniformMatrix2fv(location, count, transpose, value);
        public static void UniformMatrix2x3dv(int location, int count, bool transpose, double* value) => _glUniformMatrix2x3dv(location, count, transpose, value);
        public static void UniformMatrix2x3fv(int location, int count, bool transpose, float* value) => _glUniformMatrix2x3fv(location, count, transpose, value);
        public static void UniformMatrix2x4dv(int location, int count, bool transpose, double* value) => _glUniformMatrix2x4dv(location, count, transpose, value);
        public static void UniformMatrix2x4fv(int location, int count, bool transpose, float* value) => _glUniformMatrix2x4fv(location, count, transpose, value);
        public static void UniformMatrix3dv(int location, int count, bool transpose, double* value) => _glUniformMatrix3dv(location, count, transpose, value);
        public static void UniformMatrix3fv(int location, int count, bool transpose, float* value) => _glUniformMatrix3fv(location, count, transpose, value);
        public static void UniformMatrix3x2dv(int location, int count, bool transpose, double* value) => _glUniformMatrix3x2dv(location, count, transpose, value);
        public static void UniformMatrix3x2fv(int location, int count, bool transpose, float* value) => _glUniformMatrix3x2fv(location, count, transpose, value);
        public static void UniformMatrix3x4dv(int location, int count, bool transpose, double* value) => _glUniformMatrix3x4dv(location, count, transpose, value);
        public static void UniformMatrix3x4fv(int location, int count, bool transpose, float* value) => _glUniformMatrix3x4fv(location, count, transpose, value);
        public static void UniformMatrix4dv(int location, int count, bool transpose, double* value) => _glUniformMatrix4dv(location, count, transpose, value);
        public static void UniformMatrix4fv(int location, int count, bool transpose, float* value) => _glUniformMatrix4fv(location, count, transpose, value);
        public static void UniformMatrix4x2dv(int location, int count, bool transpose, double* value) => _glUniformMatrix4x2dv(location, count, transpose, value);
        public static void UniformMatrix4x2fv(int location, int count, bool transpose, float* value) => _glUniformMatrix4x2fv(location, count, transpose, value);
        public static void UniformMatrix4x3dv(int location, int count, bool transpose, double* value) => _glUniformMatrix4x3dv(location, count, transpose, value);
        public static void UniformMatrix4x3fv(int location, int count, bool transpose, float* value) => _glUniformMatrix4x3fv(location, count, transpose, value);
        public static void UniformSubroutinesuiv(SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, int count, uint* indices) => _glUniformSubroutinesuiv(shadertype, count, indices);
        public static bool UnmapBuffer(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target) => _glUnmapBuffer(target);
        public static bool UnmapNamedBuffer(uint buffer) => _glUnmapNamedBuffer(buffer);
        public static void UseProgram(uint program) => _glUseProgram(program);
        public static void UseProgramStages(uint pipeline, SpiceEngine.GLFWBindings.GLEnums.UseProgramStageMask stages, uint program) => _glUseProgramStages(pipeline, stages, program);
        public static void ValidateProgram(uint program) => _glValidateProgram(program);
        public static void ValidateProgramPipeline(uint pipeline) => _glValidateProgramPipeline(pipeline);
        public static void Vertex2d(double x, double y) => _glVertex2d(x, y);
        public static void Vertex2dv(double* v) => _glVertex2dv(v);
        public static void Vertex2f(float x, float y) => _glVertex2f(x, y);
        public static void Vertex2fv(float* v) => _glVertex2fv(v);
        public static void Vertex2i(int x, int y) => _glVertex2i(x, y);
        public static void Vertex2iv(int* v) => _glVertex2iv(v);
        public static void Vertex2s(short x, short y) => _glVertex2s(x, y);
        public static void Vertex2sv(short* v) => _glVertex2sv(v);
        public static void Vertex3d(double x, double y, double z) => _glVertex3d(x, y, z);
        public static void Vertex3dv(double* v) => _glVertex3dv(v);
        public static void Vertex3f(float x, float y, float z) => _glVertex3f(x, y, z);
        public static void Vertex3fv(float* v) => _glVertex3fv(v);
        public static void Vertex3i(int x, int y, int z) => _glVertex3i(x, y, z);
        public static void Vertex3iv(int* v) => _glVertex3iv(v);
        public static void Vertex3s(short x, short y, short z) => _glVertex3s(x, y, z);
        public static void Vertex3sv(short* v) => _glVertex3sv(v);
        public static void Vertex4d(double x, double y, double z, double w) => _glVertex4d(x, y, z, w);
        public static void Vertex4dv(double* v) => _glVertex4dv(v);
        public static void Vertex4f(float x, float y, float z, float w) => _glVertex4f(x, y, z, w);
        public static void Vertex4fv(float* v) => _glVertex4fv(v);
        public static void Vertex4i(int x, int y, int z, int w) => _glVertex4i(x, y, z, w);
        public static void Vertex4iv(int* v) => _glVertex4iv(v);
        public static void Vertex4s(short x, short y, short z, short w) => _glVertex4s(x, y, z, w);
        public static void Vertex4sv(short* v) => _glVertex4sv(v);
        public static void VertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex) => _glVertexArrayAttribBinding(vaobj, attribindex, bindingindex);
        public static void VertexArrayAttribFormat(uint vaobj, uint attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribType type, bool normalized, uint relativeoffset) => _glVertexArrayAttribFormat(vaobj, attribindex, size, type, normalized, relativeoffset);
        public static void VertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribIType type, uint relativeoffset) => _glVertexArrayAttribIFormat(vaobj, attribindex, size, type, relativeoffset);
        public static void VertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribLType type, uint relativeoffset) => _glVertexArrayAttribLFormat(vaobj, attribindex, size, type, relativeoffset);
        public static void VertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor) => _glVertexArrayBindingDivisor(vaobj, bindingindex, divisor);
        public static void VertexArrayElementBuffer(uint vaobj, uint buffer) => _glVertexArrayElementBuffer(vaobj, buffer);
        public static void VertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, int stride) => _glVertexArrayVertexBuffer(vaobj, bindingindex, buffer, offset, stride);
        public static void VertexArrayVertexBuffers(uint vaobj, uint first, int count, uint* buffers, IntPtr* offsets, int* strides) => _glVertexArrayVertexBuffers(vaobj, first, count, buffers, offsets, strides);
        public static void VertexAttrib1d(uint index, double x) => _glVertexAttrib1d(index, x);
        public static void VertexAttrib1dv(uint index, double* v) => _glVertexAttrib1dv(index, v);
        public static void VertexAttrib1f(uint index, float x) => _glVertexAttrib1f(index, x);
        public static void VertexAttrib1fv(uint index, float* v) => _glVertexAttrib1fv(index, v);
        public static void VertexAttrib1s(uint index, short x) => _glVertexAttrib1s(index, x);
        public static void VertexAttrib1sv(uint index, short* v) => _glVertexAttrib1sv(index, v);
        public static void VertexAttrib2d(uint index, double x, double y) => _glVertexAttrib2d(index, x, y);
        public static void VertexAttrib2dv(uint index, double* v) => _glVertexAttrib2dv(index, v);
        public static void VertexAttrib2f(uint index, float x, float y) => _glVertexAttrib2f(index, x, y);
        public static void VertexAttrib2fv(uint index, float* v) => _glVertexAttrib2fv(index, v);
        public static void VertexAttrib2s(uint index, short x, short y) => _glVertexAttrib2s(index, x, y);
        public static void VertexAttrib2sv(uint index, short* v) => _glVertexAttrib2sv(index, v);
        public static void VertexAttrib3d(uint index, double x, double y, double z) => _glVertexAttrib3d(index, x, y, z);
        public static void VertexAttrib3dv(uint index, double* v) => _glVertexAttrib3dv(index, v);
        public static void VertexAttrib3f(uint index, float x, float y, float z) => _glVertexAttrib3f(index, x, y, z);
        public static void VertexAttrib3fv(uint index, float* v) => _glVertexAttrib3fv(index, v);
        public static void VertexAttrib3s(uint index, short x, short y, short z) => _glVertexAttrib3s(index, x, y, z);
        public static void VertexAttrib3sv(uint index, short* v) => _glVertexAttrib3sv(index, v);
        public static void VertexAttrib4bv(uint index, byte* v) => _glVertexAttrib4bv(index, v);
        public static void VertexAttrib4d(uint index, double x, double y, double z, double w) => _glVertexAttrib4d(index, x, y, z, w);
        public static void VertexAttrib4dv(uint index, double* v) => _glVertexAttrib4dv(index, v);
        public static void VertexAttrib4f(uint index, float x, float y, float z, float w) => _glVertexAttrib4f(index, x, y, z, w);
        public static void VertexAttrib4fv(uint index, float* v) => _glVertexAttrib4fv(index, v);
        public static void VertexAttrib4iv(uint index, int* v) => _glVertexAttrib4iv(index, v);
        public static void VertexAttrib4Nbv(uint index, byte* v) => _glVertexAttrib4Nbv(index, v);
        public static void VertexAttrib4Niv(uint index, int* v) => _glVertexAttrib4Niv(index, v);
        public static void VertexAttrib4Nsv(uint index, short* v) => _glVertexAttrib4Nsv(index, v);
        public static void VertexAttrib4Nub(uint index, byte x, byte y, byte z, byte w) => _glVertexAttrib4Nub(index, x, y, z, w);
        public static void VertexAttrib4Nubv(uint index, byte* v) => _glVertexAttrib4Nubv(index, v);
        public static void VertexAttrib4Nuiv(uint index, uint* v) => _glVertexAttrib4Nuiv(index, v);
        public static void VertexAttrib4Nusv(uint index, ushort* v) => _glVertexAttrib4Nusv(index, v);
        public static void VertexAttrib4s(uint index, short x, short y, short z, short w) => _glVertexAttrib4s(index, x, y, z, w);
        public static void VertexAttrib4sv(uint index, short* v) => _glVertexAttrib4sv(index, v);
        public static void VertexAttrib4ubv(uint index, byte* v) => _glVertexAttrib4ubv(index, v);
        public static void VertexAttrib4uiv(uint index, uint* v) => _glVertexAttrib4uiv(index, v);
        public static void VertexAttrib4usv(uint index, ushort* v) => _glVertexAttrib4usv(index, v);
        public static void VertexAttribBinding(uint attribindex, uint bindingindex) => _glVertexAttribBinding(attribindex, bindingindex);
        public static void VertexAttribDivisor(uint index, uint divisor) => _glVertexAttribDivisor(index, divisor);
        public static void VertexAttribFormat(uint attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribType type, bool normalized, uint relativeoffset) => _glVertexAttribFormat(attribindex, size, type, normalized, relativeoffset);
        public static void VertexAttribI1i(uint index, int x) => _glVertexAttribI1i(index, x);
        public static void VertexAttribI1iv(uint index, int* v) => _glVertexAttribI1iv(index, v);
        public static void VertexAttribI1ui(uint index, uint x) => _glVertexAttribI1ui(index, x);
        public static void VertexAttribI1uiv(uint index, uint* v) => _glVertexAttribI1uiv(index, v);
        public static void VertexAttribI2i(uint index, int x, int y) => _glVertexAttribI2i(index, x, y);
        public static void VertexAttribI2iv(uint index, int* v) => _glVertexAttribI2iv(index, v);
        public static void VertexAttribI2ui(uint index, uint x, uint y) => _glVertexAttribI2ui(index, x, y);
        public static void VertexAttribI2uiv(uint index, uint* v) => _glVertexAttribI2uiv(index, v);
        public static void VertexAttribI3i(uint index, int x, int y, int z) => _glVertexAttribI3i(index, x, y, z);
        public static void VertexAttribI3iv(uint index, int* v) => _glVertexAttribI3iv(index, v);
        public static void VertexAttribI3ui(uint index, uint x, uint y, uint z) => _glVertexAttribI3ui(index, x, y, z);
        public static void VertexAttribI3uiv(uint index, uint* v) => _glVertexAttribI3uiv(index, v);
        public static void VertexAttribI4bv(uint index, byte* v) => _glVertexAttribI4bv(index, v);
        public static void VertexAttribI4i(uint index, int x, int y, int z, int w) => _glVertexAttribI4i(index, x, y, z, w);
        public static void VertexAttribI4iv(uint index, int* v) => _glVertexAttribI4iv(index, v);
        public static void VertexAttribI4sv(uint index, short* v) => _glVertexAttribI4sv(index, v);
        public static void VertexAttribI4ubv(uint index, byte* v) => _glVertexAttribI4ubv(index, v);
        public static void VertexAttribI4ui(uint index, uint x, uint y, uint z, uint w) => _glVertexAttribI4ui(index, x, y, z, w);
        public static void VertexAttribI4uiv(uint index, uint* v) => _glVertexAttribI4uiv(index, v);
        public static void VertexAttribI4usv(uint index, ushort* v) => _glVertexAttribI4usv(index, v);
        public static void VertexAttribIFormat(uint attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribIType type, uint relativeoffset) => _glVertexAttribIFormat(attribindex, size, type, relativeoffset);
        public static void VertexAttribIPointer(uint index, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribIType type, int stride, void* pointer) => _glVertexAttribIPointer(index, size, type, stride, pointer);
        public static void VertexAttribL1d(uint index, double x) => _glVertexAttribL1d(index, x);
        public static void VertexAttribL1dv(uint index, double* v) => _glVertexAttribL1dv(index, v);
        public static void VertexAttribL2d(uint index, double x, double y) => _glVertexAttribL2d(index, x, y);
        public static void VertexAttribL2dv(uint index, double* v) => _glVertexAttribL2dv(index, v);
        public static void VertexAttribL3d(uint index, double x, double y, double z) => _glVertexAttribL3d(index, x, y, z);
        public static void VertexAttribL3dv(uint index, double* v) => _glVertexAttribL3dv(index, v);
        public static void VertexAttribL4d(uint index, double x, double y, double z, double w) => _glVertexAttribL4d(index, x, y, z, w);
        public static void VertexAttribL4dv(uint index, double* v) => _glVertexAttribL4dv(index, v);
        public static void VertexAttribLFormat(uint attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribLType type, uint relativeoffset) => _glVertexAttribLFormat(attribindex, size, type, relativeoffset);
        public static void VertexAttribLPointer(uint index, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribLType type, int stride, void* pointer) => _glVertexAttribLPointer(index, size, type, stride, pointer);
        public static void VertexAttribP1ui(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, uint value) => _glVertexAttribP1ui(index, type, normalized, value);
        public static void VertexAttribP1uiv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, uint* value) => _glVertexAttribP1uiv(index, type, normalized, value);
        public static void VertexAttribP2ui(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, uint value) => _glVertexAttribP2ui(index, type, normalized, value);
        public static void VertexAttribP2uiv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, uint* value) => _glVertexAttribP2uiv(index, type, normalized, value);
        public static void VertexAttribP3ui(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, uint value) => _glVertexAttribP3ui(index, type, normalized, value);
        public static void VertexAttribP3uiv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, uint* value) => _glVertexAttribP3uiv(index, type, normalized, value);
        public static void VertexAttribP4ui(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, uint value) => _glVertexAttribP4ui(index, type, normalized, value);
        public static void VertexAttribP4uiv(uint index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, uint* value) => _glVertexAttribP4uiv(index, type, normalized, value);
        public static void VertexAttribPointer(uint index, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int stride, void* pointer) => _glVertexAttribPointer(index, size, type, normalized, stride, pointer);
        public static void VertexBindingDivisor(uint bindingindex, uint divisor) => _glVertexBindingDivisor(bindingindex, divisor);
        public static void VertexP2ui(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, uint value) => _glVertexP2ui(type, value);
        public static void VertexP2uiv(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, uint* value) => _glVertexP2uiv(type, value);
        public static void VertexP3ui(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, uint value) => _glVertexP3ui(type, value);
        public static void VertexP3uiv(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, uint* value) => _glVertexP3uiv(type, value);
        public static void VertexP4ui(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, uint value) => _glVertexP4ui(type, value);
        public static void VertexP4uiv(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, uint* value) => _glVertexP4uiv(type, value);
        public static void VertexPointer(int size, SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, int stride, void* pointer) => _glVertexPointer(size, type, stride, pointer);
        public static void Viewport(int x, int y, int width, int height) => _glViewport(x, y, width, height);
        public static void ViewportArrayv(uint first, int count, float* v) => _glViewportArrayv(first, count, v);
        public static void ViewportIndexedf(uint index, float x, float y, float w, float h) => _glViewportIndexedf(index, x, y, w, h);
        public static void ViewportIndexedfv(uint index, float* v) => _glViewportIndexedfv(index, v);
        public static void WaitSync(SpiceEngine.GLFWBindings.GLStructs.Sync* sync, SpiceEngine.GLFWBindings.GLEnums.SyncBehaviorFlags flags, ulong timeout) => _glWaitSync(sync, flags, timeout);
        public static void WindowPos2d(double x, double y) => _glWindowPos2d(x, y);
        public static void WindowPos2dv(double* v) => _glWindowPos2dv(v);
        public static void WindowPos2f(float x, float y) => _glWindowPos2f(x, y);
        public static void WindowPos2fv(float* v) => _glWindowPos2fv(v);
        public static void WindowPos2i(int x, int y) => _glWindowPos2i(x, y);
        public static void WindowPos2iv(int* v) => _glWindowPos2iv(v);
        public static void WindowPos2s(short x, short y) => _glWindowPos2s(x, y);
        public static void WindowPos2sv(short* v) => _glWindowPos2sv(v);
        public static void WindowPos3d(double x, double y, double z) => _glWindowPos3d(x, y, z);
        public static void WindowPos3dv(double* v) => _glWindowPos3dv(v);
        public static void WindowPos3f(float x, float y, float z) => _glWindowPos3f(x, y, z);
        public static void WindowPos3fv(float* v) => _glWindowPos3fv(v);
        public static void WindowPos3i(int x, int y, int z) => _glWindowPos3i(x, y, z);
        public static void WindowPos3iv(int* v) => _glWindowPos3iv(v);
        public static void WindowPos3s(short x, short y, short z) => _glWindowPos3s(x, y, z);
        public static void WindowPos3sv(short* v) => _glWindowPos3sv(v);

        public static void ActiveShaderProgram(int pipeline, int program) => _glActiveShaderProgram((uint)pipeline, (uint)program);

        public static bool AreTexturesResident(int n, int[] textures, bool* residences)
        {
            unsafe
            {
                fixed (int* texturesPtr = &textures[0])
                {
                    return _glAreTexturesResident(n, (uint*)texturesPtr, residences);
                }
            }
        }

        public static void AttachShader(int program, int shader) => _glAttachShader((uint)program, (uint)shader);

        public static void BeginConditionalRender(int id, SpiceEngine.GLFWBindings.GLEnums.ConditionalRenderMode mode) => _glBeginConditionalRender((uint)id, mode);

        public static void BeginQuery(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, int id) => _glBeginQuery(target, (uint)id);

        public static void BeginQueryIndexed(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, int index, int id) => _glBeginQueryIndexed(target, (uint)index, (uint)id);

        public static void BindAttribLocation(int program, int index, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    _glBindAttribLocation((uint)program, (uint)index, (char*)namePtr);
                }
            }
        }

        public static void BindBuffer(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, int buffer) => _glBindBuffer(target, (uint)buffer);

        public static void BindBufferBase(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, int index, int buffer) => _glBindBufferBase(target, (uint)index, (uint)buffer);

        public static void BindBufferRange(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, int index, int buffer, IntPtr offset, IntPtr size) => _glBindBufferRange(target, (uint)index, (uint)buffer, offset, size);

        public static void BindBuffersBase(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, int first, int count, int[] buffers)
        {
            unsafe
            {
                fixed (int* buffersPtr = &buffers[0])
                {
                    _glBindBuffersBase(target, (uint)first, count, (uint*)buffersPtr);
                }
            }
        }

        public static void BindBuffersRange(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, int first, int count, int[] buffers, IntPtr* offsets, IntPtr* sizes)
        {
            unsafe
            {
                fixed (int* buffersPtr = &buffers[0])
                {
                    _glBindBuffersRange(target, (uint)first, count, (uint*)buffersPtr, offsets, sizes);
                }
            }
        }

        public static void BindFragDataLocation(int program, int color, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    _glBindFragDataLocation((uint)program, (uint)color, (char*)namePtr);
                }
            }
        }

        public static void BindFragDataLocationIndexed(int program, int colorNumber, int index, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    _glBindFragDataLocationIndexed((uint)program, (uint)colorNumber, (uint)index, (char*)namePtr);
                }
            }
        }

        public static void BindFramebuffer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, int framebuffer) => _glBindFramebuffer(target, (uint)framebuffer);

        public static void BindImageTexture(int unit, int texture, int level, bool layered, int layer, SpiceEngine.GLFWBindings.GLEnums.BufferAccessARB access, SpiceEngine.GLFWBindings.GLEnums.InternalFormat format) => _glBindImageTexture((uint)unit, (uint)texture, level, layered, layer, access, format);

        public static void BindImageTextures(int first, int count, int[] textures)
        {
            unsafe
            {
                fixed (int* texturesPtr = &textures[0])
                {
                    _glBindImageTextures((uint)first, count, (uint*)texturesPtr);
                }
            }
        }

        public static void BindProgramPipeline(int pipeline) => _glBindProgramPipeline((uint)pipeline);

        public static void BindRenderbuffer(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget target, int renderbuffer) => _glBindRenderbuffer(target, (uint)renderbuffer);

        public static void BindSampler(int unit, int sampler) => _glBindSampler((uint)unit, (uint)sampler);

        public static void BindSamplers(int first, int count, int[] samplers)
        {
            unsafe
            {
                fixed (int* samplersPtr = &samplers[0])
                {
                    _glBindSamplers((uint)first, count, (uint*)samplersPtr);
                }
            }
        }

        public static void BindTexture(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int texture) => _glBindTexture(target, (uint)texture);

        public static void BindTextures(int first, int count, int[] textures)
        {
            unsafe
            {
                fixed (int* texturesPtr = &textures[0])
                {
                    _glBindTextures((uint)first, count, (uint*)texturesPtr);
                }
            }
        }

        public static void BindTextureUnit(int unit, int texture) => _glBindTextureUnit((uint)unit, (uint)texture);

        public static void BindTransformFeedback(SpiceEngine.GLFWBindings.GLEnums.BindTransformFeedbackTarget target, int id) => _glBindTransformFeedback(target, (uint)id);

        public static void BindVertexArray(int array) => _glBindVertexArray((uint)array);

        public static void BindVertexBuffer(int bindingindex, int buffer, IntPtr offset, int stride) => _glBindVertexBuffer((uint)bindingindex, (uint)buffer, offset, stride);

        public static void BindVertexBuffers(int first, int count, int[] buffers, IntPtr* offsets, int[] strides)
        {
            unsafe
            {
                fixed (int* buffersPtr = &buffers[0])
                {
                    fixed (int* stridesPtr = &strides[0])
                    {
                        _glBindVertexBuffers((uint)first, count, (uint*)buffersPtr, offsets, stridesPtr);
                    }
                }
            }
        }

        public static void BlendEquationi(int buf, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT mode) => _glBlendEquationi((uint)buf, mode);

        public static void BlendEquationSeparatei(int buf, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT modeRGB, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT modeAlpha) => _glBlendEquationSeparatei((uint)buf, modeRGB, modeAlpha);

        public static void BlendFunci(int buf, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor src, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor dst) => _glBlendFunci((uint)buf, src, dst);

        public static void BlendFuncSeparatei(int buf, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor srcRGB, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor dstRGB, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor srcAlpha, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor dstAlpha) => _glBlendFuncSeparatei((uint)buf, srcRGB, dstRGB, srcAlpha, dstAlpha);

        public static void BlitNamedFramebuffer(int readFramebuffer, int drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, SpiceEngine.GLFWBindings.GLEnums.ClearBufferMask mask, SpiceEngine.GLFWBindings.GLEnums.BlitFramebufferFilter filter) => _glBlitNamedFramebuffer((uint)readFramebuffer, (uint)drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);

        public static void BufferData(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, IntPtr size, IntPtr data, SpiceEngine.GLFWBindings.GLEnums.BufferUsageARB usage) => _glBufferData(target, size, data.ToPointer(), usage);

        public static void BufferStorage(SpiceEngine.GLFWBindings.GLEnums.BufferStorageTarget target, IntPtr size, IntPtr data, SpiceEngine.GLFWBindings.GLEnums.BufferStorageMask flags) => _glBufferStorage(target, size, data.ToPointer(), flags);

        public static void BufferSubData(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, IntPtr offset, IntPtr size, IntPtr data) => _glBufferSubData(target, offset, size, data.ToPointer());

        public static void CallList(int list) => _glCallList((uint)list);

        public static void CallList(SpiceEngine.GLFWBindings.GLEnums.ListNameType type, IntPtr lists) => CallLists(1, type, lists);

        public static void CallLists(int n, SpiceEngine.GLFWBindings.GLEnums.ListNameType type, IntPtr lists) => _glCallLists(n, type, lists.ToPointer());

        public static SpiceEngine.GLFWBindings.GLEnums.FramebufferStatus CheckNamedFramebufferStatus(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target) => _glCheckNamedFramebufferStatus((uint)framebuffer, target);

        public static void ClearBufferData(SpiceEngine.GLFWBindings.GLEnums.BufferStorageTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr data) => _glClearBufferData(target, internalformat, format, type, data.ToPointer());

        public static void ClearBufferfv(SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glClearBufferfv(buffer, drawbuffer, valuePtr);
                }
            }
        }

        public static void ClearBufferiv(SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glClearBufferiv(buffer, drawbuffer, valuePtr);
                }
            }
        }

        public static void ClearBufferSubData(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, IntPtr offset, IntPtr size, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr data) => _glClearBufferSubData(target, internalformat, offset, size, format, type, data.ToPointer());

        public static void ClearBufferuiv(SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glClearBufferuiv(buffer, drawbuffer, (uint*)valuePtr);
                }
            }
        }

        public static void ClearNamedBufferData(int buffer, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr data) => _glClearNamedBufferData((uint)buffer, internalformat, format, type, data.ToPointer());

        public static void ClearNamedBufferSubData(int buffer, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, IntPtr offset, IntPtr size, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr data) => _glClearNamedBufferSubData((uint)buffer, internalformat, offset, size, format, type, data.ToPointer());

        public static void ClearNamedFramebufferfi(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, float depth, int stencil) => _glClearNamedFramebufferfi((uint)framebuffer, buffer, drawbuffer, depth, stencil);

        public static void ClearNamedFramebufferfv(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glClearNamedFramebufferfv((uint)framebuffer, buffer, drawbuffer, valuePtr);
                }
            }
        }

        public static void ClearNamedFramebufferiv(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glClearNamedFramebufferiv((uint)framebuffer, buffer, drawbuffer, valuePtr);
                }
            }
        }

        public static void ClearNamedFramebufferuiv(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.Buffer buffer, int drawbuffer, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glClearNamedFramebufferuiv((uint)framebuffer, buffer, drawbuffer, (uint*)valuePtr);
                }
            }
        }

        public static void ClearTexImage(int texture, int level, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr data) => _glClearTexImage((uint)texture, level, format, type, data.ToPointer());

        public static void ClearTexSubImage(int texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr data) => _glClearTexSubImage((uint)texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, data.ToPointer());

        public static void Color3fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glColor3fv(vPtr);
                }
            }
        }

        public static void Color3iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glColor3iv(vPtr);
                }
            }
        }

        public static void Color3ui(int red, int green, int blue) => _glColor3ui((uint)red, (uint)green, (uint)blue);

        public static void Color3uiv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glColor3uiv((uint*)vPtr);
                }
            }
        }

        public static void Color4fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glColor4fv(vPtr);
                }
            }
        }

        public static void Color4iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glColor4iv(vPtr);
                }
            }
        }

        public static void Color4ui(int red, int green, int blue, int alpha) => _glColor4ui((uint)red, (uint)green, (uint)blue, (uint)alpha);

        public static void Color4uiv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glColor4uiv((uint*)vPtr);
                }
            }
        }

        public static void ColorMaski(int index, bool r, bool g, bool b, bool a) => _glColorMaski((uint)index, r, g, b, a);

        public static void ColorP3ui(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int color) => _glColorP3ui(type, (uint)color);

        public static void ColorP3uiv(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int[] color)
        {
            unsafe
            {
                fixed (int* colorPtr = &color[0])
                {
                    _glColorP3uiv(type, (uint*)colorPtr);
                }
            }
        }

        public static void ColorP4ui(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int color) => _glColorP4ui(type, (uint)color);

        public static void ColorP4uiv(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int[] color)
        {
            unsafe
            {
                fixed (int* colorPtr = &color[0])
                {
                    _glColorP4uiv(type, (uint*)colorPtr);
                }
            }
        }

        public static void ColorPointer(int size, SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int stride, IntPtr pointer) => _glColorPointer(size, type, stride, pointer.ToPointer());

        public static void CompileShader(int shader) => _glCompileShader((uint)shader);

        public static void CompressedTexImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int border, int imageSize, IntPtr data) => _glCompressedTexImage1D(target, level, internalformat, width, border, imageSize, data.ToPointer());

        public static void CompressedTexImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int border, int imageSize, IntPtr data) => _glCompressedTexImage2D(target, level, internalformat, width, height, border, imageSize, data.ToPointer());

        public static void CompressedTexImage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data) => _glCompressedTexImage3D(target, level, internalformat, width, height, depth, border, imageSize, data.ToPointer());

        public static void CompressedTexSubImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int width, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, IntPtr data) => _glCompressedTexSubImage1D(target, level, xoffset, width, format, imageSize, data.ToPointer());

        public static void CompressedTexSubImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, IntPtr data) => _glCompressedTexSubImage2D(target, level, xoffset, yoffset, width, height, format, imageSize, data.ToPointer());

        public static void CompressedTexSubImage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, IntPtr data) => _glCompressedTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data.ToPointer());

        public static void CompressedTextureSubImage1D(int texture, int level, int xoffset, int width, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, IntPtr data) => _glCompressedTextureSubImage1D((uint)texture, level, xoffset, width, format, imageSize, data.ToPointer());

        public static void CompressedTextureSubImage2D(int texture, int level, int xoffset, int yoffset, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, IntPtr data) => _glCompressedTextureSubImage2D((uint)texture, level, xoffset, yoffset, width, height, format, imageSize, data.ToPointer());

        public static void CompressedTextureSubImage3D(int texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, int imageSize, IntPtr data) => _glCompressedTextureSubImage3D((uint)texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data.ToPointer());

        public static void CopyImageSubData(int srcName, SpiceEngine.GLFWBindings.GLEnums.CopyImageSubDataTarget srcTarget, int srcLevel, int srcX, int srcY, int srcZ, int dstName, SpiceEngine.GLFWBindings.GLEnums.CopyImageSubDataTarget dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth) => _glCopyImageSubData((uint)srcName, srcTarget, srcLevel, srcX, srcY, srcZ, (uint)dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);

        public static void CopyNamedBufferSubData(int readBuffer, int writeBuffer, IntPtr readOffset, IntPtr writeOffset, IntPtr size) => _glCopyNamedBufferSubData((uint)readBuffer, (uint)writeBuffer, readOffset, writeOffset, size);

        public static void CopyTextureSubImage1D(int texture, int level, int xoffset, int x, int y, int width) => _glCopyTextureSubImage1D((uint)texture, level, xoffset, x, y, width);

        public static void CopyTextureSubImage2D(int texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) => _glCopyTextureSubImage2D((uint)texture, level, xoffset, yoffset, x, y, width, height);

        public static void CopyTextureSubImage3D(int texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => _glCopyTextureSubImage3D((uint)texture, level, xoffset, yoffset, zoffset, x, y, width, height);

        public static void CreateBuffer(int buffer)
        {
            var buffers = new int[] { buffer };
            CreateBuffers(1, buffers);
        }

        public static void CreateBuffers(int n, int[] buffers)
        {
            unsafe
            {
                fixed (int* buffersPtr = &buffers[0])
                {
                    _glCreateBuffers(n, (uint*)buffersPtr);
                }
            }
        }

        public static void CreateFramebuffer(int framebuffer)
        {
            var framebuffers = new int[] { framebuffer };
            CreateFramebuffers(1, framebuffers);
        }

        public static void CreateFramebuffers(int n, int[] framebuffers)
        {
            unsafe
            {
                fixed (int* framebuffersPtr = &framebuffers[0])
                {
                    _glCreateFramebuffers(n, (uint*)framebuffersPtr);
                }
            }
        }

        public static int CreateProgram() => (int)_glCreateProgram();

        public static void CreateProgramPipeline(int pipeline)
        {
            var pipelines = new int[] { pipeline };
            CreateProgramPipelines(1, pipelines);
        }

        public static void CreateProgramPipelines(int n, int[] pipelines)
        {
            unsafe
            {
                fixed (int* pipelinesPtr = &pipelines[0])
                {
                    _glCreateProgramPipelines(n, (uint*)pipelinesPtr);
                }
            }
        }

        public static void CreateQueries(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, int n, int[] ids)
        {
            unsafe
            {
                fixed (int* idsPtr = &ids[0])
                {
                    _glCreateQueries(target, n, (uint*)idsPtr);
                }
            }
        }

        public static void CreateRenderbuffer(int renderbuffer)
        {
            var renderbuffers = new int[] { renderbuffer };
            CreateRenderbuffers(1, renderbuffers);
        }

        public static void CreateRenderbuffers(int n, int[] renderbuffers)
        {
            unsafe
            {
                fixed (int* renderbuffersPtr = &renderbuffers[0])
                {
                    _glCreateRenderbuffers(n, (uint*)renderbuffersPtr);
                }
            }
        }

        public static void CreateSampler(int sampler)
        {
            var samplers = new int[] { sampler };
            CreateSamplers(1, samplers);
        }

        public static void CreateSamplers(int n, int[] samplers)
        {
            unsafe
            {
                fixed (int* samplersPtr = &samplers[0])
                {
                    _glCreateSamplers(n, (uint*)samplersPtr);
                }
            }
        }

        public static int CreateShader(SpiceEngine.GLFWBindings.GLEnums.ShaderType type) => (int)_glCreateShader(type);

        public static int CreateShaderProgramv(SpiceEngine.GLFWBindings.GLEnums.ShaderType type, int count, string[] strings)
        {
            var ptrs = new List<IntPtr>();
            var size = Marshal.SizeOf(typeof(IntPtr));
            var stringsPtr = Marshal.AllocHGlobal(size * strings.Length);
            
            for (var i = 0; i < strings.Length; i++)
            {
                var stringsSinglePtr = Marshal.StringToHGlobalAnsi(strings[i]);
                ptrs.Add(stringsSinglePtr);
                Marshal.WriteIntPtr(stringsPtr, i * size, stringsSinglePtr);
            }
            
            return (int)_glCreateShaderProgramv(type, count, (char**)stringsPtr);
            
            Marshal.FreeHGlobal(stringsPtr);
            
            foreach (var ptr in ptrs)
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static void CreateTextures(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int n, int[] textures)
        {
            unsafe
            {
                fixed (int* texturesPtr = &textures[0])
                {
                    _glCreateTextures(target, n, (uint*)texturesPtr);
                }
            }
        }

        public static void CreateTransformFeedback(int id)
        {
            var ids = new int[] { id };
            CreateTransformFeedbacks(1, ids);
        }

        public static void CreateTransformFeedbacks(int n, int[] ids)
        {
            unsafe
            {
                fixed (int* idsPtr = &ids[0])
                {
                    _glCreateTransformFeedbacks(n, (uint*)idsPtr);
                }
            }
        }

        public static void CreateVertexArray(int array)
        {
            var arrays = new int[] { array };
            CreateVertexArrays(1, arrays);
        }

        public static void CreateVertexArrays(int n, int[] arrays)
        {
            unsafe
            {
                fixed (int* arraysPtr = &arrays[0])
                {
                    _glCreateVertexArrays(n, (uint*)arraysPtr);
                }
            }
        }

        public static void DebugMessageCallback(IntPtr callback, IntPtr userParam) => _glDebugMessageCallback(callback.ToPointer(), userParam.ToPointer());

        public static void DebugMessageControl(SpiceEngine.GLFWBindings.GLEnums.DebugSource source, SpiceEngine.GLFWBindings.GLEnums.DebugType type, SpiceEngine.GLFWBindings.GLEnums.DebugSeverity severity, int count, int[] ids, bool enabled)
        {
            unsafe
            {
                fixed (int* idsPtr = &ids[0])
                {
                    _glDebugMessageControl(source, type, severity, count, (uint*)idsPtr, enabled);
                }
            }
        }

        public static void DebugMessageInsert(SpiceEngine.GLFWBindings.GLEnums.DebugSource source, SpiceEngine.GLFWBindings.GLEnums.DebugType type, int id, SpiceEngine.GLFWBindings.GLEnums.DebugSeverity severity, int length, string buf)
        {
            unsafe
            {
                var bufBytes = Encoding.UTF8.GetBytes(buf);
                fixed (byte* bufPtr = &bufBytes[0])
                {
                    _glDebugMessageInsert(source, type, (uint)id, severity, length, (char*)bufPtr);
                }
            }
        }

        public static void DeleteBuffer(int buffer)
        {
            var buffers = new int[] { buffer };
            DeleteBuffers(1, buffers);
        }

        public static void DeleteBuffers(int n, int[] buffers)
        {
            unsafe
            {
                fixed (int* buffersPtr = &buffers[0])
                {
                    _glDeleteBuffers(n, (uint*)buffersPtr);
                }
            }
        }

        public static void DeleteFramebuffer(int framebuffer)
        {
            var framebuffers = new int[] { framebuffer };
            DeleteFramebuffers(1, framebuffers);
        }

        public static void DeleteFramebuffers(int n, int[] framebuffers)
        {
            unsafe
            {
                fixed (int* framebuffersPtr = &framebuffers[0])
                {
                    _glDeleteFramebuffers(n, (uint*)framebuffersPtr);
                }
            }
        }

        public static void DeleteLists(int list, int range) => _glDeleteLists((uint)list, range);

        public static void DeleteProgram(int program) => _glDeleteProgram((uint)program);

        public static void DeleteProgramPipeline(int pipeline)
        {
            var pipelines = new int[] { pipeline };
            DeleteProgramPipelines(1, pipelines);
        }

        public static void DeleteProgramPipelines(int n, int[] pipelines)
        {
            unsafe
            {
                fixed (int* pipelinesPtr = &pipelines[0])
                {
                    _glDeleteProgramPipelines(n, (uint*)pipelinesPtr);
                }
            }
        }

        public static void DeleteQueries(int n, int[] ids)
        {
            unsafe
            {
                fixed (int* idsPtr = &ids[0])
                {
                    _glDeleteQueries(n, (uint*)idsPtr);
                }
            }
        }

        public static void DeleteQuery(int id)
        {
            var ids = new int[] { id };
            DeleteQueries(1, ids);
        }

        public static void DeleteRenderbuffer(int renderbuffer)
        {
            var renderbuffers = new int[] { renderbuffer };
            DeleteRenderbuffers(1, renderbuffers);
        }

        public static void DeleteRenderbuffers(int n, int[] renderbuffers)
        {
            unsafe
            {
                fixed (int* renderbuffersPtr = &renderbuffers[0])
                {
                    _glDeleteRenderbuffers(n, (uint*)renderbuffersPtr);
                }
            }
        }

        public static void DeleteSampler(int sampler)
        {
            var samplers = new int[] { sampler };
            DeleteSamplers(1, samplers);
        }

        public static void DeleteSamplers(int count, int[] samplers)
        {
            unsafe
            {
                fixed (int* samplersPtr = &samplers[0])
                {
                    _glDeleteSamplers(count, (uint*)samplersPtr);
                }
            }
        }

        public static void DeleteShader(int shader) => _glDeleteShader((uint)shader);

        public static void DeleteTexture(int texture)
        {
            var textures = new int[] { texture };
            DeleteTextures(1, textures);
        }

        public static void DeleteTextures(int n, int[] textures)
        {
            unsafe
            {
                fixed (int* texturesPtr = &textures[0])
                {
                    _glDeleteTextures(n, (uint*)texturesPtr);
                }
            }
        }

        public static void DeleteTransformFeedback(int id)
        {
            var ids = new int[] { id };
            DeleteTransformFeedbacks(1, ids);
        }

        public static void DeleteTransformFeedbacks(int n, int[] ids)
        {
            unsafe
            {
                fixed (int* idsPtr = &ids[0])
                {
                    _glDeleteTransformFeedbacks(n, (uint*)idsPtr);
                }
            }
        }

        public static void DeleteVertexArray(int array)
        {
            var arrays = new int[] { array };
            DeleteVertexArrays(1, arrays);
        }

        public static void DeleteVertexArrays(int n, int[] arrays)
        {
            unsafe
            {
                fixed (int* arraysPtr = &arrays[0])
                {
                    _glDeleteVertexArrays(n, (uint*)arraysPtr);
                }
            }
        }

        public static void DepthRangeArrayv(int first, int count, double* v) => _glDepthRangeArrayv((uint)first, count, v);

        public static void DepthRangeIndexed(int index, double n, double f) => _glDepthRangeIndexed((uint)index, n, f);

        public static void DetachShader(int program, int shader) => _glDetachShader((uint)program, (uint)shader);

        public static void Disablei(SpiceEngine.GLFWBindings.GLEnums.EnableCap target, int index) => _glDisablei(target, (uint)index);

        public static void DisableVertexArrayAttrib(int vaobj, int index) => _glDisableVertexArrayAttrib((uint)vaobj, (uint)index);

        public static void DisableVertexAttribArray(int index) => _glDisableVertexAttribArray((uint)index);

        public static void DispatchCompute(int num_groups_x, int num_groups_y, int num_groups_z) => _glDispatchCompute((uint)num_groups_x, (uint)num_groups_y, (uint)num_groups_z);

        public static void DrawArraysIndirect(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, IntPtr indirect) => _glDrawArraysIndirect(mode, indirect.ToPointer());

        public static void DrawArraysInstancedBaseInstance(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int first, int count, int instancecount, int baseinstance) => _glDrawArraysInstancedBaseInstance(mode, first, count, instancecount, (uint)baseinstance);

        public static void DrawBuffers(int n, SpiceEngine.GLFWBindings.GLEnums.DrawBufferMode[] bufs)
        {
            unsafe
            {
                fixed (SpiceEngine.GLFWBindings.GLEnums.DrawBufferMode* bufsPtr = &bufs[0])
                {
                    _glDrawBuffers(n, bufsPtr);
                }
            }
        }

        public static void DrawElements(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indices) => _glDrawElements(mode, count, type, indices.ToPointer());

        public static void DrawElementsBaseVertex(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indices, int basevertex) => _glDrawElementsBaseVertex(mode, count, type, indices.ToPointer(), basevertex);

        public static void DrawElementsIndirect(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indirect) => _glDrawElementsIndirect(mode, type, indirect.ToPointer());

        public static void DrawElementsInstanced(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indices, int instancecount) => _glDrawElementsInstanced(mode, count, type, indices.ToPointer(), instancecount);

        public static void DrawElementsInstancedBaseInstance(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.PrimitiveType type, IntPtr indices, int instancecount, int baseinstance) => _glDrawElementsInstancedBaseInstance(mode, count, type, indices.ToPointer(), instancecount, (uint)baseinstance);

        public static void DrawElementsInstancedBaseVertex(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indices, int instancecount, int basevertex) => _glDrawElementsInstancedBaseVertex(mode, count, type, indices.ToPointer(), instancecount, basevertex);

        public static void DrawElementsInstancedBaseVertexBaseInstance(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indices, int instancecount, int basevertex, int baseinstance) => _glDrawElementsInstancedBaseVertexBaseInstance(mode, count, type, indices.ToPointer(), instancecount, basevertex, (uint)baseinstance);

        public static void DrawPixels(int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glDrawPixels(width, height, format, type, pixels.ToPointer());

        public static void DrawRangeElements(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int start, int end, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indices) => _glDrawRangeElements(mode, (uint)start, (uint)end, count, type, indices.ToPointer());

        public static void DrawRangeElementsBaseVertex(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int start, int end, int count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indices, int basevertex) => _glDrawRangeElementsBaseVertex(mode, (uint)start, (uint)end, count, type, indices.ToPointer(), basevertex);

        public static void DrawTransformFeedback(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int id) => _glDrawTransformFeedback(mode, (uint)id);

        public static void DrawTransformFeedbackInstanced(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int id, int instancecount) => _glDrawTransformFeedbackInstanced(mode, (uint)id, instancecount);

        public static void DrawTransformFeedbackStream(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int id, int stream) => _glDrawTransformFeedbackStream(mode, (uint)id, (uint)stream);

        public static void DrawTransformFeedbackStreamInstanced(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int id, int stream, int instancecount) => _glDrawTransformFeedbackStreamInstanced(mode, (uint)id, (uint)stream, instancecount);

        public static void EdgeFlagPointer(int stride, IntPtr pointer) => _glEdgeFlagPointer(stride, pointer.ToPointer());

        public static void Enablei(SpiceEngine.GLFWBindings.GLEnums.EnableCap target, int index) => _glEnablei(target, (uint)index);

        public static void EnableVertexArrayAttrib(int vaobj, int index) => _glEnableVertexArrayAttrib((uint)vaobj, (uint)index);

        public static void EnableVertexAttribArray(int index) => _glEnableVertexAttribArray((uint)index);

        public static void EndQueryIndexed(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, int index) => _glEndQueryIndexed(target, (uint)index);

        public static void EvalCoord1fv(float[] u)
        {
            unsafe
            {
                fixed (float* uPtr = &u[0])
                {
                    _glEvalCoord1fv(uPtr);
                }
            }
        }

        public static void EvalCoord2fv(float[] u)
        {
            unsafe
            {
                fixed (float* uPtr = &u[0])
                {
                    _glEvalCoord2fv(uPtr);
                }
            }
        }

        public static void FeedbackBuffer(int size, SpiceEngine.GLFWBindings.GLEnums.FeedbackType type, float[] buffer)
        {
            unsafe
            {
                fixed (float* bufferPtr = &buffer[0])
                {
                    _glFeedbackBuffer(size, type, bufferPtr);
                }
            }
        }

        public static void FlushMappedNamedBufferRange(int buffer, IntPtr offset, IntPtr length) => _glFlushMappedNamedBufferRange((uint)buffer, offset, length);

        public static void FogCoordfv(float[] coord)
        {
            unsafe
            {
                fixed (float* coordPtr = &coord[0])
                {
                    _glFogCoordfv(coordPtr);
                }
            }
        }

        public static void FogCoordPointer(SpiceEngine.GLFWBindings.GLEnums.FogPointerTypeEXT type, int stride, IntPtr pointer) => _glFogCoordPointer(type, stride, pointer.ToPointer());

        public static void Fogfv(SpiceEngine.GLFWBindings.GLEnums.FogParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glFogfv(pname, @paramsPtr);
                }
            }
        }

        public static void Fogiv(SpiceEngine.GLFWBindings.GLEnums.FogParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glFogiv(pname, @paramsPtr);
                }
            }
        }

        public static void FramebufferRenderbuffer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget renderbuffertarget, int renderbuffer) => _glFramebufferRenderbuffer(target, attachment, renderbuffertarget, (uint)renderbuffer);

        public static void FramebufferTexture(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, int texture, int level) => _glFramebufferTexture(target, attachment, (uint)texture, level);

        public static void FramebufferTexture1D(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.TextureTarget textarget, int texture, int level) => _glFramebufferTexture1D(target, attachment, textarget, (uint)texture, level);

        public static void FramebufferTexture2D(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.TextureTarget textarget, int texture, int level) => _glFramebufferTexture2D(target, attachment, textarget, (uint)texture, level);

        public static void FramebufferTexture3D(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.TextureTarget textarget, int texture, int level, int zoffset) => _glFramebufferTexture3D(target, attachment, textarget, (uint)texture, level, zoffset);

        public static void FramebufferTextureLayer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, int texture, int level, int layer) => _glFramebufferTextureLayer(target, attachment, (uint)texture, level, layer);

        public static int GenBuffer()
        {
            var values = GenBuffers(1);
            return values[0];
        }

        public static void GenBuffers(int n, int[] buffers)
        {
            unsafe
            {
                fixed (int* buffersPtr = &buffers[0])
                {
                    _glGenBuffers(n, (uint*)buffersPtr);
                }
            }
        }

        public static int[] GenBuffers(int n)
        {
            var values = new int[n];
            GenBuffers(n, values);
            return values;
        }

        public static void GenerateTextureMipmap(int texture) => _glGenerateTextureMipmap((uint)texture);

        public static int GenFramebuffer()
        {
            var values = GenFramebuffers(1);
            return values[0];
        }

        public static void GenFramebuffers(int n, int[] framebuffers)
        {
            unsafe
            {
                fixed (int* framebuffersPtr = &framebuffers[0])
                {
                    _glGenFramebuffers(n, (uint*)framebuffersPtr);
                }
            }
        }

        public static int[] GenFramebuffers(int n)
        {
            var values = new int[n];
            GenFramebuffers(n, values);
            return values;
        }

        public static int GenLists(int range) => (int)_glGenLists(range);

        public static int GenProgramPipeline()
        {
            var values = GenProgramPipelines(1);
            return values[0];
        }

        public static void GenProgramPipelines(int n, int[] pipelines)
        {
            unsafe
            {
                fixed (int* pipelinesPtr = &pipelines[0])
                {
                    _glGenProgramPipelines(n, (uint*)pipelinesPtr);
                }
            }
        }

        public static int[] GenProgramPipelines(int n)
        {
            var values = new int[n];
            GenProgramPipelines(n, values);
            return values;
        }

        public static void GenQueries(int n, int[] ids)
        {
            unsafe
            {
                fixed (int* idsPtr = &ids[0])
                {
                    _glGenQueries(n, (uint*)idsPtr);
                }
            }
        }

        public static int[] GenQueries(int n)
        {
            var values = new int[n];
            GenQueries(n, values);
            return values;
        }

        public static int GenQuery()
        {
            var values = GenQueries(1);
            return values[0];
        }

        public static int GenRenderbuffer()
        {
            var values = GenRenderbuffers(1);
            return values[0];
        }

        public static void GenRenderbuffers(int n, int[] renderbuffers)
        {
            unsafe
            {
                fixed (int* renderbuffersPtr = &renderbuffers[0])
                {
                    _glGenRenderbuffers(n, (uint*)renderbuffersPtr);
                }
            }
        }

        public static int[] GenRenderbuffers(int n)
        {
            var values = new int[n];
            GenRenderbuffers(n, values);
            return values;
        }

        public static int GenSampler()
        {
            var values = GenSamplers(1);
            return values[0];
        }

        public static int[] GenSamplers(int count)
        {
            var values = new int[count];
            GenSamplers(count, values);
            return values;
        }

        public static void GenSamplers(int count, int[] samplers)
        {
            unsafe
            {
                fixed (int* samplersPtr = &samplers[0])
                {
                    _glGenSamplers(count, (uint*)samplersPtr);
                }
            }
        }

        public static int GenTexture()
        {
            var values = GenTextures(1);
            return values[0];
        }

        public static int[] GenTextures(int n)
        {
            var values = new int[n];
            GenTextures(n, values);
            return values;
        }

        public static void GenTextures(int n, int[] textures)
        {
            unsafe
            {
                fixed (int* texturesPtr = &textures[0])
                {
                    _glGenTextures(n, (uint*)texturesPtr);
                }
            }
        }

        public static int GenTransformFeedback()
        {
            var values = GenTransformFeedbacks(1);
            return values[0];
        }

        public static void GenTransformFeedbacks(int n, int[] ids)
        {
            unsafe
            {
                fixed (int* idsPtr = &ids[0])
                {
                    _glGenTransformFeedbacks(n, (uint*)idsPtr);
                }
            }
        }

        public static int[] GenTransformFeedbacks(int n)
        {
            var values = new int[n];
            GenTransformFeedbacks(n, values);
            return values;
        }

        public static int GenVertexArray()
        {
            var values = GenVertexArrays(1);
            return values[0];
        }

        public static void GenVertexArrays(int n, int[] arrays)
        {
            unsafe
            {
                fixed (int* arraysPtr = &arrays[0])
                {
                    _glGenVertexArrays(n, (uint*)arraysPtr);
                }
            }
        }

        public static int[] GenVertexArrays(int n)
        {
            var values = new int[n];
            GenVertexArrays(n, values);
            return values;
        }

        public static int[] GetActiveAtomicCounterBufferiv(int program, int bufferIndex, SpiceEngine.GLFWBindings.GLEnums.AtomicCounterBufferPName pname)
        {
            var values = new int[1];
            GetActiveAtomicCounterBufferiv(program, bufferIndex, pname, values);
            return values;
        }

        public static void GetActiveAtomicCounterBufferiv(int program, int bufferIndex, SpiceEngine.GLFWBindings.GLEnums.AtomicCounterBufferPName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetActiveAtomicCounterBufferiv((uint)program, (uint)bufferIndex, pname, @paramsPtr);
                }
            }
        }

        public static void GetActiveAttrib(int program, int index, int bufSize, int[] length, int[] size, SpiceEngine.GLFWBindings.GLEnums.AttributeType[] type, string name)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    fixed (int* sizePtr = &size[0])
                    {
                        fixed (SpiceEngine.GLFWBindings.GLEnums.AttributeType* typePtr = &type[0])
                        {
                            var nameBytes = Encoding.UTF8.GetBytes(name);
                            fixed (byte* namePtr = &nameBytes[0])
                            {
                                _glGetActiveAttrib((uint)program, (uint)index, bufSize, lengthPtr, sizePtr, typePtr, (char*)namePtr);
                            }
                        }
                    }
                }
            }
        }

        public static void GetActiveSubroutineName(int program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, int index, int bufSize, int[] length, string name)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var nameBytes = Encoding.UTF8.GetBytes(name);
                    fixed (byte* namePtr = &nameBytes[0])
                    {
                        _glGetActiveSubroutineName((uint)program, shadertype, (uint)index, bufSize, lengthPtr, (char*)namePtr);
                    }
                }
            }
        }

        public static int[] GetActiveSubroutineUniformiv(int program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, int index, SpiceEngine.GLFWBindings.GLEnums.SubroutineParameterName pname)
        {
            var values = new int[1];
            GetActiveSubroutineUniformiv(program, shadertype, index, pname, values);
            return values;
        }

        public static void GetActiveSubroutineUniformiv(int program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, int index, SpiceEngine.GLFWBindings.GLEnums.SubroutineParameterName pname, int[] values)
        {
            unsafe
            {
                fixed (int* valuesPtr = &values[0])
                {
                    _glGetActiveSubroutineUniformiv((uint)program, shadertype, (uint)index, pname, valuesPtr);
                }
            }
        }

        public static void GetActiveSubroutineUniformName(int program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, int index, int bufSize, int[] length, string name)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var nameBytes = Encoding.UTF8.GetBytes(name);
                    fixed (byte* namePtr = &nameBytes[0])
                    {
                        _glGetActiveSubroutineUniformName((uint)program, shadertype, (uint)index, bufSize, lengthPtr, (char*)namePtr);
                    }
                }
            }
        }

        public static void GetActiveUniform(int program, int index, int bufSize, int[] length, int[] size, SpiceEngine.GLFWBindings.GLEnums.UniformType[] type, string name)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    fixed (int* sizePtr = &size[0])
                    {
                        fixed (SpiceEngine.GLFWBindings.GLEnums.UniformType* typePtr = &type[0])
                        {
                            var nameBytes = Encoding.UTF8.GetBytes(name);
                            fixed (byte* namePtr = &nameBytes[0])
                            {
                                _glGetActiveUniform((uint)program, (uint)index, bufSize, lengthPtr, sizePtr, typePtr, (char*)namePtr);
                            }
                        }
                    }
                }
            }
        }

        public static void GetActiveUniformBlockiv(int program, int uniformBlockIndex, SpiceEngine.GLFWBindings.GLEnums.UniformBlockPName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetActiveUniformBlockiv((uint)program, (uint)uniformBlockIndex, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetActiveUniformBlockiv(int program, int uniformBlockIndex, SpiceEngine.GLFWBindings.GLEnums.UniformBlockPName pname)
        {
            var values = new int[1];
            GetActiveUniformBlockiv(program, uniformBlockIndex, pname, values);
            return values;
        }

        public static void GetActiveUniformBlockName(int program, int uniformBlockIndex, int bufSize, int[] length, string uniformBlockName)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var uniformBlockNameBytes = Encoding.UTF8.GetBytes(uniformBlockName);
                    fixed (byte* uniformBlockNamePtr = &uniformBlockNameBytes[0])
                    {
                        _glGetActiveUniformBlockName((uint)program, (uint)uniformBlockIndex, bufSize, lengthPtr, (char*)uniformBlockNamePtr);
                    }
                }
            }
        }

        public static void GetActiveUniformName(int program, int uniformIndex, int bufSize, int[] length, string uniformName)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var uniformNameBytes = Encoding.UTF8.GetBytes(uniformName);
                    fixed (byte* uniformNamePtr = &uniformNameBytes[0])
                    {
                        _glGetActiveUniformName((uint)program, (uint)uniformIndex, bufSize, lengthPtr, (char*)uniformNamePtr);
                    }
                }
            }
        }

        public static void GetActiveUniformsiv(int program, int uniformCount, int[] uniformIndices, SpiceEngine.GLFWBindings.GLEnums.UniformPName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* uniformIndicesPtr = &uniformIndices[0])
                {
                    fixed (int* @paramsPtr = &@params[0])
                    {
                        _glGetActiveUniformsiv((uint)program, uniformCount, (uint*)uniformIndicesPtr, pname, @paramsPtr);
                    }
                }
            }
        }

        public static int[] GetActiveUniformsiv(int program, int uniformCount, int[] uniformIndices, SpiceEngine.GLFWBindings.GLEnums.UniformPName pname)
        {
            var values = new int[1];
            GetActiveUniformsiv(program, uniformCount, uniformIndices, pname, values);
            return values;
        }

        public static void GetAttachedShaders(int program, int maxCount, int[] count, int[] shaders)
        {
            unsafe
            {
                fixed (int* countPtr = &count[0])
                {
                    fixed (int* shadersPtr = &shaders[0])
                    {
                        _glGetAttachedShaders((uint)program, maxCount, countPtr, (uint*)shadersPtr);
                    }
                }
            }
        }

        public static int[] GetAttachedShaders(int program, int maxCount, int[] count)
        {
            var values = new int[1];
            GetAttachedShaders(program, maxCount, count, values);
            return values;
        }

        public static int GetAttribLocation(int program, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    return _glGetAttribLocation((uint)program, (char*)namePtr);
                }
            }
        }

        public static void GetBooleani_v(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, int index, bool* data) => _glGetBooleani_v(target, (uint)index, data);

        public static void GetBufferParameteriv(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetBufferParameteriv(target, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetBufferParameteriv(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB pname)
        {
            var values = new int[1];
            GetBufferParameteriv(target, pname, values);
            return values;
        }

        public static void GetBufferSubData(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, IntPtr offset, IntPtr size, IntPtr data) => _glGetBufferSubData(target, offset, size, data.ToPointer());

        public static void GetCompressedTexImage(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, IntPtr img) => _glGetCompressedTexImage(target, level, img.ToPointer());

        public static void GetCompressedTextureImage(int texture, int level, int bufSize, IntPtr pixels) => _glGetCompressedTextureImage((uint)texture, level, bufSize, pixels.ToPointer());

        public static void GetCompressedTextureSubImage(int texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, IntPtr pixels) => _glGetCompressedTextureSubImage((uint)texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels.ToPointer());

        public static int GetDebugMessageLog(int count, int bufSize, SpiceEngine.GLFWBindings.GLEnums.DebugSource[] sources, SpiceEngine.GLFWBindings.GLEnums.DebugType[] types, int[] ids, SpiceEngine.GLFWBindings.GLEnums.DebugSeverity[] severities, int[] lengths, string messageLog)
        {
            unsafe
            {
                fixed (SpiceEngine.GLFWBindings.GLEnums.DebugSource* sourcesPtr = &sources[0])
                {
                    fixed (SpiceEngine.GLFWBindings.GLEnums.DebugType* typesPtr = &types[0])
                    {
                        fixed (int* idsPtr = &ids[0])
                        {
                            fixed (SpiceEngine.GLFWBindings.GLEnums.DebugSeverity* severitiesPtr = &severities[0])
                            {
                                fixed (int* lengthsPtr = &lengths[0])
                                {
                                    var messageLogBytes = Encoding.UTF8.GetBytes(messageLog);
                                    fixed (byte* messageLogPtr = &messageLogBytes[0])
                                    {
                                        return (int)_glGetDebugMessageLog((uint)count, bufSize, sourcesPtr, typesPtr, (uint*)idsPtr, severitiesPtr, lengthsPtr, (char*)messageLogPtr);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void GetDoublei_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, int index, double* data) => _glGetDoublei_v(target, (uint)index, data);

        public static void GetFloati_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, int index, float[] data)
        {
            unsafe
            {
                fixed (float* dataPtr = &data[0])
                {
                    _glGetFloati_v(target, (uint)index, dataPtr);
                }
            }
        }

        public static float[] GetFloati_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, int index)
        {
            var values = new float[1];
            GetFloati_v(target, index, values);
            return values;
        }

        public static void GetFloatv(SpiceEngine.GLFWBindings.GLEnums.GetPName pname, float[] data)
        {
            unsafe
            {
                fixed (float* dataPtr = &data[0])
                {
                    _glGetFloatv(pname, dataPtr);
                }
            }
        }

        public static float[] GetFloatv(SpiceEngine.GLFWBindings.GLEnums.GetPName pname)
        {
            var values = new float[1];
            GetFloatv(pname, values);
            return values;
        }

        public static int GetFragDataIndex(int program, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    return _glGetFragDataIndex((uint)program, (char*)namePtr);
                }
            }
        }

        public static int GetFragDataLocation(int program, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    return _glGetFragDataLocation((uint)program, (char*)namePtr);
                }
            }
        }

        public static void GetFramebufferAttachmentParameteriv(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetFramebufferAttachmentParameteriv(target, attachment, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetFramebufferAttachmentParameteriv(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName pname)
        {
            var values = new int[1];
            GetFramebufferAttachmentParameteriv(target, attachment, pname, values);
            return values;
        }

        public static void GetFramebufferParameteriv(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetFramebufferParameteriv(target, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetFramebufferParameteriv(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName pname)
        {
            var values = new int[1];
            GetFramebufferParameteriv(target, pname, values);
            return values;
        }

        public static void GetInteger64i_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, int index, long* data) => _glGetInteger64i_v(target, (uint)index, data);

        public static void GetIntegeri_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, int index, int[] data)
        {
            unsafe
            {
                fixed (int* dataPtr = &data[0])
                {
                    _glGetIntegeri_v(target, (uint)index, dataPtr);
                }
            }
        }

        public static int[] GetIntegeri_v(SpiceEngine.GLFWBindings.GLEnums.GetPName target, int index)
        {
            var values = new int[1];
            GetIntegeri_v(target, index, values);
            return values;
        }

        public static int[] GetIntegerv(SpiceEngine.GLFWBindings.GLEnums.GetPName pname)
        {
            var values = new int[1];
            GetIntegerv(pname, values);
            return values;
        }

        public static void GetIntegerv(SpiceEngine.GLFWBindings.GLEnums.GetPName pname, int[] data)
        {
            unsafe
            {
                fixed (int* dataPtr = &data[0])
                {
                    _glGetIntegerv(pname, dataPtr);
                }
            }
        }

        public static void GetInternalformativ(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, SpiceEngine.GLFWBindings.GLEnums.InternalFormatPName pname, int count, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetInternalformativ(target, internalformat, pname, count, @paramsPtr);
                }
            }
        }

        public static int[] GetInternalformativ(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, SpiceEngine.GLFWBindings.GLEnums.InternalFormatPName pname, int count)
        {
            var values = new int[count];
            GetInternalformativ(target, internalformat, pname, count, values);
            return values;
        }

        public static void GetLightfv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetLightfv(light, pname, @paramsPtr);
                }
            }
        }

        public static float[] GetLightfv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname)
        {
            var values = new float[1];
            GetLightfv(light, pname, values);
            return values;
        }

        public static void GetLightiv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetLightiv(light, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetLightiv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname)
        {
            var values = new int[1];
            GetLightiv(light, pname, values);
            return values;
        }

        public static void GetMapfv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery query, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glGetMapfv(target, query, vPtr);
                }
            }
        }

        public static float[] GetMapfv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery query)
        {
            var values = new float[1];
            GetMapfv(target, query, values);
            return values;
        }

        public static void GetMapiv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery query, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glGetMapiv(target, query, vPtr);
                }
            }
        }

        public static int[] GetMapiv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery query)
        {
            var values = new int[1];
            GetMapiv(target, query, values);
            return values;
        }

        public static void GetMaterialfv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetMaterialfv(face, pname, @paramsPtr);
                }
            }
        }

        public static float[] GetMaterialfv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname)
        {
            var values = new float[1];
            GetMaterialfv(face, pname, values);
            return values;
        }

        public static void GetMaterialiv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetMaterialiv(face, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetMaterialiv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname)
        {
            var values = new int[1];
            GetMaterialiv(face, pname, values);
            return values;
        }

        public static float[] GetMultisamplefv(SpiceEngine.GLFWBindings.GLEnums.GetMultisamplePNameNV pname, int index)
        {
            var values = new float[1];
            GetMultisamplefv(pname, index, values);
            return values;
        }

        public static void GetMultisamplefv(SpiceEngine.GLFWBindings.GLEnums.GetMultisamplePNameNV pname, int index, float[] val)
        {
            unsafe
            {
                fixed (float* valPtr = &val[0])
                {
                    _glGetMultisamplefv(pname, (uint)index, valPtr);
                }
            }
        }

        public static void GetNamedBufferParameteri64v(int buffer, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB pname, long* @params) => _glGetNamedBufferParameteri64v((uint)buffer, pname, @params);

        public static void GetNamedBufferParameteriv(int buffer, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetNamedBufferParameteriv((uint)buffer, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetNamedBufferParameteriv(int buffer, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB pname)
        {
            var values = new int[1];
            GetNamedBufferParameteriv(buffer, pname, values);
            return values;
        }

        public static void GetNamedBufferPointerv(int buffer, SpiceEngine.GLFWBindings.GLEnums.BufferPointerNameARB pname, void** @params) => _glGetNamedBufferPointerv((uint)buffer, pname, @params);

        public static void GetNamedBufferSubData(int buffer, IntPtr offset, IntPtr size, IntPtr data) => _glGetNamedBufferSubData((uint)buffer, offset, size, data.ToPointer());

        public static void GetNamedFramebufferAttachmentParameteriv(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetNamedFramebufferAttachmentParameteriv((uint)framebuffer, attachment, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetNamedFramebufferAttachmentParameteriv(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName pname)
        {
            var values = new int[1];
            GetNamedFramebufferAttachmentParameteriv(framebuffer, attachment, pname, values);
            return values;
        }

        public static void GetNamedFramebufferParameteriv(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.GetFramebufferParameter pname, int[] param)
        {
            unsafe
            {
                fixed (int* paramPtr = &param[0])
                {
                    _glGetNamedFramebufferParameteriv((uint)framebuffer, pname, paramPtr);
                }
            }
        }

        public static int[] GetNamedFramebufferParameteriv(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.GetFramebufferParameter pname)
        {
            var values = new int[1];
            GetNamedFramebufferParameteriv(framebuffer, pname, values);
            return values;
        }

        public static void GetNamedRenderbufferParameteriv(int renderbuffer, SpiceEngine.GLFWBindings.GLEnums.RenderbufferParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetNamedRenderbufferParameteriv((uint)renderbuffer, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetNamedRenderbufferParameteriv(int renderbuffer, SpiceEngine.GLFWBindings.GLEnums.RenderbufferParameterName pname)
        {
            var values = new int[1];
            GetNamedRenderbufferParameteriv(renderbuffer, pname, values);
            return values;
        }

        public static void GetnColorTable(SpiceEngine.GLFWBindings.GLEnums.ColorTableTarget target, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, IntPtr table) => _glGetnColorTable(target, format, type, bufSize, table.ToPointer());

        public static void GetnCompressedTexImage(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int lod, int bufSize, IntPtr pixels) => _glGetnCompressedTexImage(target, lod, bufSize, pixels.ToPointer());

        public static void GetnConvolutionFilter(SpiceEngine.GLFWBindings.GLEnums.ConvolutionTarget target, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, IntPtr image) => _glGetnConvolutionFilter(target, format, type, bufSize, image.ToPointer());

        public static void GetnHistogram(SpiceEngine.GLFWBindings.GLEnums.HistogramTarget target, bool reset, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, IntPtr values) => _glGetnHistogram(target, reset, format, type, bufSize, values.ToPointer());

        public static float[] GetnMapfv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.MapQuery query, int bufSize)
        {
            var values = new float[1];
            GetnMapfv(target, query, bufSize, values);
            return values;
        }

        public static void GetnMapfv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.MapQuery query, int bufSize, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glGetnMapfv(target, query, bufSize, vPtr);
                }
            }
        }

        public static void GetnMapiv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.MapQuery query, int bufSize, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glGetnMapiv(target, query, bufSize, vPtr);
                }
            }
        }

        public static int[] GetnMapiv(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, SpiceEngine.GLFWBindings.GLEnums.MapQuery query, int bufSize)
        {
            var values = new int[1];
            GetnMapiv(target, query, bufSize, values);
            return values;
        }

        public static void GetnMinmax(SpiceEngine.GLFWBindings.GLEnums.MinmaxTarget target, bool reset, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, IntPtr values) => _glGetnMinmax(target, reset, format, type, bufSize, values.ToPointer());

        public static void GetnPixelMapfv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int bufSize, float[] values)
        {
            unsafe
            {
                fixed (float* valuesPtr = &values[0])
                {
                    _glGetnPixelMapfv(map, bufSize, valuesPtr);
                }
            }
        }

        public static float[] GetnPixelMapfv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int bufSize)
        {
            var values = new float[1];
            GetnPixelMapfv(map, bufSize, values);
            return values;
        }

        public static void GetnPixelMapuiv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int bufSize, int[] values)
        {
            unsafe
            {
                fixed (int* valuesPtr = &values[0])
                {
                    _glGetnPixelMapuiv(map, bufSize, (uint*)valuesPtr);
                }
            }
        }

        public static int[] GetnPixelMapuiv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int bufSize)
        {
            var values = new int[1];
            GetnPixelMapuiv(map, bufSize, values);
            return values;
        }

        public static void GetnSeparableFilter(SpiceEngine.GLFWBindings.GLEnums.SeparableTarget target, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int rowBufSize, IntPtr row, int columnBufSize, IntPtr column, IntPtr span) => _glGetnSeparableFilter(target, format, type, rowBufSize, row.ToPointer(), columnBufSize, column.ToPointer(), span.ToPointer());

        public static void GetnTexImage(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, IntPtr pixels) => _glGetnTexImage(target, level, format, type, bufSize, pixels.ToPointer());

        public static void GetnUniformdv(int program, int location, int bufSize, double* @params) => _glGetnUniformdv((uint)program, location, bufSize, @params);

        public static float[] GetnUniformfv(int program, int location, int bufSize)
        {
            var values = new float[1];
            GetnUniformfv(program, location, bufSize, values);
            return values;
        }

        public static void GetnUniformfv(int program, int location, int bufSize, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetnUniformfv((uint)program, location, bufSize, @paramsPtr);
                }
            }
        }

        public static void GetnUniformiv(int program, int location, int bufSize, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetnUniformiv((uint)program, location, bufSize, @paramsPtr);
                }
            }
        }

        public static int[] GetnUniformiv(int program, int location, int bufSize)
        {
            var values = new int[1];
            GetnUniformiv(program, location, bufSize, values);
            return values;
        }

        public static void GetnUniformuiv(int program, int location, int bufSize, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetnUniformuiv((uint)program, location, bufSize, (uint*)@paramsPtr);
                }
            }
        }

        public static int[] GetnUniformuiv(int program, int location, int bufSize)
        {
            var values = new int[1];
            GetnUniformuiv(program, location, bufSize, values);
            return values;
        }

        public static void GetObjectLabel(SpiceEngine.GLFWBindings.GLEnums.ObjectIdentifier identifier, int name, int bufSize, int[] length, string label)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var labelBytes = Encoding.UTF8.GetBytes(label);
                    fixed (byte* labelPtr = &labelBytes[0])
                    {
                        _glGetObjectLabel(identifier, (uint)name, bufSize, lengthPtr, (char*)labelPtr);
                    }
                }
            }
        }

        public static void GetObjectPtrLabel(IntPtr ptr, int bufSize, int[] length, string label)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var labelBytes = Encoding.UTF8.GetBytes(label);
                    fixed (byte* labelPtr = &labelBytes[0])
                    {
                        _glGetObjectPtrLabel(ptr.ToPointer(), bufSize, lengthPtr, (char*)labelPtr);
                    }
                }
            }
        }

        public static float[] GetPixelMapfv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map)
        {
            var values = new float[1];
            GetPixelMapfv(map, values);
            return values;
        }

        public static void GetPixelMapfv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, float[] values)
        {
            unsafe
            {
                fixed (float* valuesPtr = &values[0])
                {
                    _glGetPixelMapfv(map, valuesPtr);
                }
            }
        }

        public static int[] GetPixelMapuiv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map)
        {
            var values = new int[1];
            GetPixelMapuiv(map, values);
            return values;
        }

        public static void GetPixelMapuiv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int[] values)
        {
            unsafe
            {
                fixed (int* valuesPtr = &values[0])
                {
                    _glGetPixelMapuiv(map, (uint*)valuesPtr);
                }
            }
        }

        public static void GetProgramBinary(int program, int bufSize, int[] length, int[] binaryFormat, IntPtr binary)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    fixed (int* binaryFormatPtr = &binaryFormat[0])
                    {
                        _glGetProgramBinary((uint)program, bufSize, lengthPtr, binaryFormatPtr, binary.ToPointer());
                    }
                }
            }
        }

        public static void GetProgramInfoLog(int program, int bufSize, int[] length, string infoLog)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var infoLogBytes = Encoding.UTF8.GetBytes(infoLog);
                    fixed (byte* infoLogPtr = &infoLogBytes[0])
                    {
                        _glGetProgramInfoLog((uint)program, bufSize, lengthPtr, (char*)infoLogPtr);
                    }
                }
            }
        }

        public static void GetProgramInterfaceiv(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, SpiceEngine.GLFWBindings.GLEnums.ProgramInterfacePName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetProgramInterfaceiv((uint)program, programInterface, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetProgramInterfaceiv(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, SpiceEngine.GLFWBindings.GLEnums.ProgramInterfacePName pname)
        {
            var values = new int[1];
            GetProgramInterfaceiv(program, programInterface, pname, values);
            return values;
        }

        public static int[] GetProgramiv(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramPropertyARB pname)
        {
            var values = new int[1];
            GetProgramiv(program, pname, values);
            return values;
        }

        public static void GetProgramiv(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramPropertyARB pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetProgramiv((uint)program, pname, @paramsPtr);
                }
            }
        }

        public static void GetProgramPipelineInfoLog(int pipeline, int bufSize, int[] length, string infoLog)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var infoLogBytes = Encoding.UTF8.GetBytes(infoLog);
                    fixed (byte* infoLogPtr = &infoLogBytes[0])
                    {
                        _glGetProgramPipelineInfoLog((uint)pipeline, bufSize, lengthPtr, (char*)infoLogPtr);
                    }
                }
            }
        }

        public static void GetProgramPipelineiv(int pipeline, SpiceEngine.GLFWBindings.GLEnums.PipelineParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetProgramPipelineiv((uint)pipeline, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetProgramPipelineiv(int pipeline, SpiceEngine.GLFWBindings.GLEnums.PipelineParameterName pname)
        {
            var values = new int[1];
            GetProgramPipelineiv(pipeline, pname, values);
            return values;
        }

        public static int GetProgramResourceIndex(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    return (int)_glGetProgramResourceIndex((uint)program, programInterface, (char*)namePtr);
                }
            }
        }

        public static int[] GetProgramResourceiv(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, int index, int propCount, SpiceEngine.GLFWBindings.GLEnums.ProgramResourceProperty[] props, int count, int[] length)
        {
            var values = new int[count];
            GetProgramResourceiv(program, programInterface, index, propCount, props, count, length, values);
            return values;
        }

        public static void GetProgramResourceiv(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, int index, int propCount, SpiceEngine.GLFWBindings.GLEnums.ProgramResourceProperty[] props, int count, int[] length, int[] @params)
        {
            unsafe
            {
                fixed (SpiceEngine.GLFWBindings.GLEnums.ProgramResourceProperty* propsPtr = &props[0])
                {
                    fixed (int* lengthPtr = &length[0])
                    {
                        fixed (int* @paramsPtr = &@params[0])
                        {
                            _glGetProgramResourceiv((uint)program, programInterface, (uint)index, propCount, propsPtr, count, lengthPtr, @paramsPtr);
                        }
                    }
                }
            }
        }

        public static int GetProgramResourceLocation(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    return _glGetProgramResourceLocation((uint)program, programInterface, (char*)namePtr);
                }
            }
        }

        public static int GetProgramResourceLocationIndex(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    return _glGetProgramResourceLocationIndex((uint)program, programInterface, (char*)namePtr);
                }
            }
        }

        public static void GetProgramResourceName(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface programInterface, int index, int bufSize, int[] length, string name)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var nameBytes = Encoding.UTF8.GetBytes(name);
                    fixed (byte* namePtr = &nameBytes[0])
                    {
                        _glGetProgramResourceName((uint)program, programInterface, (uint)index, bufSize, lengthPtr, (char*)namePtr);
                    }
                }
            }
        }

        public static void GetProgramStageiv(int program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, SpiceEngine.GLFWBindings.GLEnums.ProgramStagePName pname, int[] values)
        {
            unsafe
            {
                fixed (int* valuesPtr = &values[0])
                {
                    _glGetProgramStageiv((uint)program, shadertype, pname, valuesPtr);
                }
            }
        }

        public static int[] GetProgramStageiv(int program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, SpiceEngine.GLFWBindings.GLEnums.ProgramStagePName pname)
        {
            var values = new int[1];
            GetProgramStageiv(program, shadertype, pname, values);
            return values;
        }

        public static void GetQueryBufferObjecti64v(int id, int buffer, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, IntPtr offset) => _glGetQueryBufferObjecti64v((uint)id, (uint)buffer, pname, offset);

        public static void GetQueryBufferObjectiv(int id, int buffer, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, IntPtr offset) => _glGetQueryBufferObjectiv((uint)id, (uint)buffer, pname, offset);

        public static void GetQueryBufferObjectui64v(int id, int buffer, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, IntPtr offset) => _glGetQueryBufferObjectui64v((uint)id, (uint)buffer, pname, offset);

        public static void GetQueryBufferObjectuiv(int id, int buffer, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, IntPtr offset) => _glGetQueryBufferObjectuiv((uint)id, (uint)buffer, pname, offset);

        public static int[] GetQueryIndexediv(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, int index, SpiceEngine.GLFWBindings.GLEnums.QueryParameterName pname)
        {
            var values = new int[1];
            GetQueryIndexediv(target, index, pname, values);
            return values;
        }

        public static void GetQueryIndexediv(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, int index, SpiceEngine.GLFWBindings.GLEnums.QueryParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetQueryIndexediv(target, (uint)index, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetQueryiv(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, SpiceEngine.GLFWBindings.GLEnums.QueryParameterName pname)
        {
            var values = new int[1];
            GetQueryiv(target, pname, values);
            return values;
        }

        public static void GetQueryiv(SpiceEngine.GLFWBindings.GLEnums.QueryTarget target, SpiceEngine.GLFWBindings.GLEnums.QueryParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetQueryiv(target, pname, @paramsPtr);
                }
            }
        }

        public static void GetQueryObjecti64v(int id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, long* @params) => _glGetQueryObjecti64v((uint)id, pname, @params);

        public static int[] GetQueryObjectiv(int id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname)
        {
            var values = new int[1];
            GetQueryObjectiv(id, pname, values);
            return values;
        }

        public static void GetQueryObjectiv(int id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetQueryObjectiv((uint)id, pname, @paramsPtr);
                }
            }
        }

        public static void GetQueryObjectui64v(int id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, ulong* @params) => _glGetQueryObjectui64v((uint)id, pname, @params);

        public static void GetQueryObjectuiv(int id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetQueryObjectuiv((uint)id, pname, (uint*)@paramsPtr);
                }
            }
        }

        public static int[] GetQueryObjectuiv(int id, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName pname)
        {
            var values = new int[1];
            GetQueryObjectuiv(id, pname, values);
            return values;
        }

        public static int[] GetRenderbufferParameteriv(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget target, SpiceEngine.GLFWBindings.GLEnums.RenderbufferParameterName pname)
        {
            var values = new int[1];
            GetRenderbufferParameteriv(target, pname, values);
            return values;
        }

        public static void GetRenderbufferParameteriv(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget target, SpiceEngine.GLFWBindings.GLEnums.RenderbufferParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetRenderbufferParameteriv(target, pname, @paramsPtr);
                }
            }
        }

        public static float[] GetSamplerParameterfv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterF pname)
        {
            var values = new float[1];
            GetSamplerParameterfv(sampler, pname, values);
            return values;
        }

        public static void GetSamplerParameterfv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterF pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetSamplerParameterfv((uint)sampler, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetSamplerParameterIiv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname)
        {
            var values = new int[1];
            GetSamplerParameterIiv(sampler, pname, values);
            return values;
        }

        public static void GetSamplerParameterIiv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetSamplerParameterIiv((uint)sampler, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetSamplerParameterIuiv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname)
        {
            var values = new int[1];
            GetSamplerParameterIuiv(sampler, pname, values);
            return values;
        }

        public static void GetSamplerParameterIuiv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetSamplerParameterIuiv((uint)sampler, pname, (uint*)@paramsPtr);
                }
            }
        }

        public static int[] GetSamplerParameteriv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname)
        {
            var values = new int[1];
            GetSamplerParameteriv(sampler, pname, values);
            return values;
        }

        public static void GetSamplerParameteriv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetSamplerParameteriv((uint)sampler, pname, @paramsPtr);
                }
            }
        }

        public static void GetShaderInfoLog(int shader, int bufSize, int[] length, string infoLog)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var infoLogBytes = Encoding.UTF8.GetBytes(infoLog);
                    fixed (byte* infoLogPtr = &infoLogBytes[0])
                    {
                        _glGetShaderInfoLog((uint)shader, bufSize, lengthPtr, (char*)infoLogPtr);
                    }
                }
            }
        }

        public static int[] GetShaderiv(int shader, SpiceEngine.GLFWBindings.GLEnums.ShaderParameterName pname)
        {
            var values = new int[1];
            GetShaderiv(shader, pname, values);
            return values;
        }

        public static void GetShaderiv(int shader, SpiceEngine.GLFWBindings.GLEnums.ShaderParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetShaderiv((uint)shader, pname, @paramsPtr);
                }
            }
        }

        public static void GetShaderPrecisionFormat(SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, SpiceEngine.GLFWBindings.GLEnums.PrecisionType precisiontype, int[] range, int[] precision)
        {
            unsafe
            {
                fixed (int* rangePtr = &range[0])
                {
                    fixed (int* precisionPtr = &precision[0])
                    {
                        _glGetShaderPrecisionFormat(shadertype, precisiontype, rangePtr, precisionPtr);
                    }
                }
            }
        }

        public static int[] GetShaderPrecisionFormat(SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, SpiceEngine.GLFWBindings.GLEnums.PrecisionType precisiontype, int[] range)
        {
            var values = new int[1];
            GetShaderPrecisionFormat(shadertype, precisiontype, range, values);
            return values;
        }

        public static void GetShaderSource(int shader, int bufSize, int[] length, string source)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    var sourceBytes = Encoding.UTF8.GetBytes(source);
                    fixed (byte* sourcePtr = &sourceBytes[0])
                    {
                        _glGetShaderSource((uint)shader, bufSize, lengthPtr, (char*)sourcePtr);
                    }
                }
            }
        }

        public static byte* GetStringi(SpiceEngine.GLFWBindings.GLEnums.StringName name, int index) => _glGetStringi(name, (uint)index);

        public static int GetSubroutineIndex(int program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    return (int)_glGetSubroutineIndex((uint)program, shadertype, (char*)namePtr);
                }
            }
        }

        public static int GetSubroutineUniformLocation(int program, SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    return _glGetSubroutineUniformLocation((uint)program, shadertype, (char*)namePtr);
                }
            }
        }

        public static void GetSynciv(SpiceEngine.GLFWBindings.GLStructs.Sync* sync, SpiceEngine.GLFWBindings.GLEnums.SyncParameterName pname, int count, int[] length, int[] values)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    fixed (int* valuesPtr = &values[0])
                    {
                        _glGetSynciv(sync, pname, count, lengthPtr, valuesPtr);
                    }
                }
            }
        }

        public static int[] GetSynciv(SpiceEngine.GLFWBindings.GLStructs.Sync* sync, SpiceEngine.GLFWBindings.GLEnums.SyncParameterName pname, int count, int[] length)
        {
            var values = new int[count];
            GetSynciv(sync, pname, count, length, values);
            return values;
        }

        public static void GetTexEnvfv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetTexEnvfv(target, pname, @paramsPtr);
                }
            }
        }

        public static float[] GetTexEnvfv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname)
        {
            var values = new float[1];
            GetTexEnvfv(target, pname, values);
            return values;
        }

        public static int[] GetTexEnviv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname)
        {
            var values = new int[1];
            GetTexEnviv(target, pname, values);
            return values;
        }

        public static void GetTexEnviv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTexEnviv(target, pname, @paramsPtr);
                }
            }
        }

        public static float[] GetTexGenfv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname)
        {
            var values = new float[1];
            GetTexGenfv(coord, pname, values);
            return values;
        }

        public static void GetTexGenfv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetTexGenfv(coord, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetTexGeniv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname)
        {
            var values = new int[1];
            GetTexGeniv(coord, pname, values);
            return values;
        }

        public static void GetTexGeniv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTexGeniv(coord, pname, @paramsPtr);
                }
            }
        }

        public static void GetTexImage(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glGetTexImage(target, level, format, type, pixels.ToPointer());

        public static float[] GetTexLevelParameterfv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new float[1];
            GetTexLevelParameterfv(target, level, pname, values);
            return values;
        }

        public static void GetTexLevelParameterfv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetTexLevelParameterfv(target, level, pname, @paramsPtr);
                }
            }
        }

        public static void GetTexLevelParameteriv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTexLevelParameteriv(target, level, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetTexLevelParameteriv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new int[1];
            GetTexLevelParameteriv(target, level, pname, values);
            return values;
        }

        public static void GetTexParameterfv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetTexParameterfv(target, pname, @paramsPtr);
                }
            }
        }

        public static float[] GetTexParameterfv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new float[1];
            GetTexParameterfv(target, pname, values);
            return values;
        }

        public static int[] GetTexParameterIiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new int[1];
            GetTexParameterIiv(target, pname, values);
            return values;
        }

        public static void GetTexParameterIiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTexParameterIiv(target, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetTexParameterIuiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new int[1];
            GetTexParameterIuiv(target, pname, values);
            return values;
        }

        public static void GetTexParameterIuiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTexParameterIuiv(target, pname, (uint*)@paramsPtr);
                }
            }
        }

        public static int[] GetTexParameteriv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new int[1];
            GetTexParameteriv(target, pname, values);
            return values;
        }

        public static void GetTexParameteriv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTexParameteriv(target, pname, @paramsPtr);
                }
            }
        }

        public static void GetTextureImage(int texture, int level, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, IntPtr pixels) => _glGetTextureImage((uint)texture, level, format, type, bufSize, pixels.ToPointer());

        public static float[] GetTextureLevelParameterfv(int texture, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new float[1];
            GetTextureLevelParameterfv(texture, level, pname, values);
            return values;
        }

        public static void GetTextureLevelParameterfv(int texture, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetTextureLevelParameterfv((uint)texture, level, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetTextureLevelParameteriv(int texture, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new int[1];
            GetTextureLevelParameteriv(texture, level, pname, values);
            return values;
        }

        public static void GetTextureLevelParameteriv(int texture, int level, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTextureLevelParameteriv((uint)texture, level, pname, @paramsPtr);
                }
            }
        }

        public static void GetTextureParameterfv(int texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetTextureParameterfv((uint)texture, pname, @paramsPtr);
                }
            }
        }

        public static float[] GetTextureParameterfv(int texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new float[1];
            GetTextureParameterfv(texture, pname, values);
            return values;
        }

        public static int[] GetTextureParameterIiv(int texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new int[1];
            GetTextureParameterIiv(texture, pname, values);
            return values;
        }

        public static void GetTextureParameterIiv(int texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTextureParameterIiv((uint)texture, pname, @paramsPtr);
                }
            }
        }

        public static int[] GetTextureParameterIuiv(int texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new int[1];
            GetTextureParameterIuiv(texture, pname, values);
            return values;
        }

        public static void GetTextureParameterIuiv(int texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTextureParameterIuiv((uint)texture, pname, (uint*)@paramsPtr);
                }
            }
        }

        public static int[] GetTextureParameteriv(int texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname)
        {
            var values = new int[1];
            GetTextureParameteriv(texture, pname, values);
            return values;
        }

        public static void GetTextureParameteriv(int texture, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetTextureParameteriv((uint)texture, pname, @paramsPtr);
                }
            }
        }

        public static void GetTextureSubImage(int texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, IntPtr pixels) => _glGetTextureSubImage((uint)texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels.ToPointer());

        public static int[] GetTransformFeedbacki_v(int xfb, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName pname, int index)
        {
            var values = new int[1];
            GetTransformFeedbacki_v(xfb, pname, index, values);
            return values;
        }

        public static void GetTransformFeedbacki_v(int xfb, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName pname, int index, int[] param)
        {
            unsafe
            {
                fixed (int* paramPtr = &param[0])
                {
                    _glGetTransformFeedbacki_v((uint)xfb, pname, (uint)index, paramPtr);
                }
            }
        }

        public static void GetTransformFeedbacki64_v(int xfb, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName pname, int index, long* param) => _glGetTransformFeedbacki64_v((uint)xfb, pname, (uint)index, param);

        public static int[] GetTransformFeedbackiv(int xfb, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName pname)
        {
            var values = new int[1];
            GetTransformFeedbackiv(xfb, pname, values);
            return values;
        }

        public static void GetTransformFeedbackiv(int xfb, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName pname, int[] param)
        {
            unsafe
            {
                fixed (int* paramPtr = &param[0])
                {
                    _glGetTransformFeedbackiv((uint)xfb, pname, paramPtr);
                }
            }
        }

        public static void GetTransformFeedbackVarying(int program, int index, int bufSize, int[] length, int[] size, SpiceEngine.GLFWBindings.GLEnums.AttributeType[] type, string name)
        {
            unsafe
            {
                fixed (int* lengthPtr = &length[0])
                {
                    fixed (int* sizePtr = &size[0])
                    {
                        fixed (SpiceEngine.GLFWBindings.GLEnums.AttributeType* typePtr = &type[0])
                        {
                            var nameBytes = Encoding.UTF8.GetBytes(name);
                            fixed (byte* namePtr = &nameBytes[0])
                            {
                                _glGetTransformFeedbackVarying((uint)program, (uint)index, bufSize, lengthPtr, sizePtr, typePtr, (char*)namePtr);
                            }
                        }
                    }
                }
            }
        }

        public static int GetUniformBlockIndex(int program, string uniformBlockName)
        {
            unsafe
            {
                var uniformBlockNameBytes = Encoding.UTF8.GetBytes(uniformBlockName);
                fixed (byte* uniformBlockNamePtr = &uniformBlockNameBytes[0])
                {
                    return (int)_glGetUniformBlockIndex((uint)program, (char*)uniformBlockNamePtr);
                }
            }
        }

        public static void GetUniformdv(int program, int location, double* @params) => _glGetUniformdv((uint)program, location, @params);

        public static void GetUniformfv(int program, int location, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetUniformfv((uint)program, location, @paramsPtr);
                }
            }
        }

        public static float[] GetUniformfv(int program, int location)
        {
            var values = new float[1];
            GetUniformfv(program, location, values);
            return values;
        }

        public static void GetUniformIndices(int program, int uniformCount, string[] uniformNames, int[] uniformIndices)
        {
            unsafe
            {
                var ptrs = new List<IntPtr>();
                var size = Marshal.SizeOf(typeof(IntPtr));
                var uniformNamesPtr = Marshal.AllocHGlobal(size * uniformNames.Length);
                
                for (var i = 0; i < uniformNames.Length; i++)
                {
                    var uniformNamesSinglePtr = Marshal.StringToHGlobalAnsi(uniformNames[i]);
                    ptrs.Add(uniformNamesSinglePtr);
                    Marshal.WriteIntPtr(uniformNamesPtr, i * size, uniformNamesSinglePtr);
                }
                
                fixed (int* uniformIndicesPtr = &uniformIndices[0])
                {
                    _glGetUniformIndices((uint)program, uniformCount, (char**)uniformNamesPtr, (uint*)uniformIndicesPtr);
                }
                
                Marshal.FreeHGlobal(uniformNamesPtr);
                
                foreach (var ptr in ptrs)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        public static int[] GetUniformIndices(int program, int uniformCount, string[] uniformNames)
        {
            var values = new int[1];
            GetUniformIndices(program, uniformCount, uniformNames, values);
            return values;
        }

        public static int[] GetUniformiv(int program, int location)
        {
            var values = new int[1];
            GetUniformiv(program, location, values);
            return values;
        }

        public static void GetUniformiv(int program, int location, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetUniformiv((uint)program, location, @paramsPtr);
                }
            }
        }

        public static int GetUniformLocation(int program, string name)
        {
            unsafe
            {
                var nameBytes = Encoding.UTF8.GetBytes(name);
                fixed (byte* namePtr = &nameBytes[0])
                {
                    return _glGetUniformLocation((uint)program, (char*)namePtr);
                }
            }
        }

        public static void GetUniformSubroutineuiv(SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, int location, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetUniformSubroutineuiv(shadertype, location, (uint*)@paramsPtr);
                }
            }
        }

        public static int[] GetUniformSubroutineuiv(SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, int location)
        {
            var values = new int[1];
            GetUniformSubroutineuiv(shadertype, location, values);
            return values;
        }

        public static void GetUniformuiv(int program, int location, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetUniformuiv((uint)program, location, (uint*)@paramsPtr);
                }
            }
        }

        public static int[] GetUniformuiv(int program, int location)
        {
            var values = new int[1];
            GetUniformuiv(program, location, values);
            return values;
        }

        public static void GetVertexArrayIndexed64iv(int vaobj, int index, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName pname, long* param) => _glGetVertexArrayIndexed64iv((uint)vaobj, (uint)index, pname, param);

        public static int[] GetVertexArrayIndexediv(int vaobj, int index, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName pname)
        {
            var values = new int[1];
            GetVertexArrayIndexediv(vaobj, index, pname, values);
            return values;
        }

        public static void GetVertexArrayIndexediv(int vaobj, int index, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName pname, int[] param)
        {
            unsafe
            {
                fixed (int* paramPtr = &param[0])
                {
                    _glGetVertexArrayIndexediv((uint)vaobj, (uint)index, pname, paramPtr);
                }
            }
        }

        public static int[] GetVertexArrayiv(int vaobj, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName pname)
        {
            var values = new int[1];
            GetVertexArrayiv(vaobj, pname, values);
            return values;
        }

        public static void GetVertexArrayiv(int vaobj, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName pname, int[] param)
        {
            unsafe
            {
                fixed (int* paramPtr = &param[0])
                {
                    _glGetVertexArrayiv((uint)vaobj, pname, paramPtr);
                }
            }
        }

        public static void GetVertexAttribdv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB pname, double* @params) => _glGetVertexAttribdv((uint)index, pname, @params);

        public static void GetVertexAttribfv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glGetVertexAttribfv((uint)index, pname, @paramsPtr);
                }
            }
        }

        public static float[] GetVertexAttribfv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB pname)
        {
            var values = new float[1];
            GetVertexAttribfv(index, pname, values);
            return values;
        }

        public static int[] GetVertexAttribIiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum pname)
        {
            var values = new int[1];
            GetVertexAttribIiv(index, pname, values);
            return values;
        }

        public static void GetVertexAttribIiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetVertexAttribIiv((uint)index, pname, @paramsPtr);
                }
            }
        }

        public static void GetVertexAttribIuiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetVertexAttribIuiv((uint)index, pname, (uint*)@paramsPtr);
                }
            }
        }

        public static int[] GetVertexAttribIuiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum pname)
        {
            var values = new int[1];
            GetVertexAttribIuiv(index, pname, values);
            return values;
        }

        public static int[] GetVertexAttribiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB pname)
        {
            var values = new int[1];
            GetVertexAttribiv(index, pname, values);
            return values;
        }

        public static void GetVertexAttribiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glGetVertexAttribiv((uint)index, pname, @paramsPtr);
                }
            }
        }

        public static void GetVertexAttribLdv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum pname, double* @params) => _glGetVertexAttribLdv((uint)index, pname, @params);

        public static void GetVertexAttribPointerv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerPropertyARB pname, void** pointer) => _glGetVertexAttribPointerv((uint)index, pname, pointer);

        public static void Indexfv(float[] c)
        {
            unsafe
            {
                fixed (float* cPtr = &c[0])
                {
                    _glIndexfv(cPtr);
                }
            }
        }

        public static void Indexiv(int[] c)
        {
            unsafe
            {
                fixed (int* cPtr = &c[0])
                {
                    _glIndexiv(cPtr);
                }
            }
        }

        public static void IndexMask(int mask) => _glIndexMask((uint)mask);

        public static void IndexPointer(SpiceEngine.GLFWBindings.GLEnums.IndexPointerType type, int stride, IntPtr pointer) => _glIndexPointer(type, stride, pointer.ToPointer());

        public static void InterleavedArrays(SpiceEngine.GLFWBindings.GLEnums.InterleavedArrayFormat format, int stride, IntPtr pointer) => _glInterleavedArrays(format, stride, pointer.ToPointer());

        public static void InvalidateBufferData(int buffer) => _glInvalidateBufferData((uint)buffer);

        public static void InvalidateBufferSubData(int buffer, IntPtr offset, IntPtr length) => _glInvalidateBufferSubData((uint)buffer, offset, length);

        public static void InvalidateFramebuffer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, int numAttachments, SpiceEngine.GLFWBindings.GLEnums.InvalidateFramebufferAttachment[] attachments)
        {
            unsafe
            {
                fixed (SpiceEngine.GLFWBindings.GLEnums.InvalidateFramebufferAttachment* attachmentsPtr = &attachments[0])
                {
                    _glInvalidateFramebuffer(target, numAttachments, attachmentsPtr);
                }
            }
        }

        public static void InvalidateNamedFramebufferData(int framebuffer, int numAttachments, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment[] attachments)
        {
            unsafe
            {
                fixed (SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment* attachmentsPtr = &attachments[0])
                {
                    _glInvalidateNamedFramebufferData((uint)framebuffer, numAttachments, attachmentsPtr);
                }
            }
        }

        public static void InvalidateNamedFramebufferSubData(int framebuffer, int numAttachments, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment[] attachments, int x, int y, int width, int height)
        {
            unsafe
            {
                fixed (SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment* attachmentsPtr = &attachments[0])
                {
                    _glInvalidateNamedFramebufferSubData((uint)framebuffer, numAttachments, attachmentsPtr, x, y, width, height);
                }
            }
        }

        public static void InvalidateSubFramebuffer(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget target, int numAttachments, SpiceEngine.GLFWBindings.GLEnums.InvalidateFramebufferAttachment[] attachments, int x, int y, int width, int height)
        {
            unsafe
            {
                fixed (SpiceEngine.GLFWBindings.GLEnums.InvalidateFramebufferAttachment* attachmentsPtr = &attachments[0])
                {
                    _glInvalidateSubFramebuffer(target, numAttachments, attachmentsPtr, x, y, width, height);
                }
            }
        }

        public static void InvalidateTexImage(int texture, int level) => _glInvalidateTexImage((uint)texture, level);

        public static void InvalidateTexSubImage(int texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth) => _glInvalidateTexSubImage((uint)texture, level, xoffset, yoffset, zoffset, width, height, depth);

        public static bool IsBuffer(int buffer) => _glIsBuffer((uint)buffer);

        public static bool IsEnabledi(SpiceEngine.GLFWBindings.GLEnums.EnableCap target, int index) => _glIsEnabledi(target, (uint)index);

        public static bool IsFramebuffer(int framebuffer) => _glIsFramebuffer((uint)framebuffer);

        public static bool IsList(int list) => _glIsList((uint)list);

        public static bool IsProgram(int program) => _glIsProgram((uint)program);

        public static bool IsProgramPipeline(int pipeline) => _glIsProgramPipeline((uint)pipeline);

        public static bool IsQuery(int id) => _glIsQuery((uint)id);

        public static bool IsRenderbuffer(int renderbuffer) => _glIsRenderbuffer((uint)renderbuffer);

        public static bool IsSampler(int sampler) => _glIsSampler((uint)sampler);

        public static bool IsShader(int shader) => _glIsShader((uint)shader);

        public static bool IsTexture(int texture) => _glIsTexture((uint)texture);

        public static bool IsTransformFeedback(int id) => _glIsTransformFeedback((uint)id);

        public static bool IsVertexArray(int array) => _glIsVertexArray((uint)array);

        public static void Lightfv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glLightfv(light, pname, @paramsPtr);
                }
            }
        }

        public static void Lightiv(SpiceEngine.GLFWBindings.GLEnums.LightName light, SpiceEngine.GLFWBindings.GLEnums.LightParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glLightiv(light, pname, @paramsPtr);
                }
            }
        }

        public static void LightModelfv(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glLightModelfv(pname, @paramsPtr);
                }
            }
        }

        public static void LightModeliv(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glLightModeliv(pname, @paramsPtr);
                }
            }
        }

        public static void LinkProgram(int program) => _glLinkProgram((uint)program);

        public static void ListBase(int @base) => _glListBase((uint)@base);

        public static void LoadMatrixf(float[] m)
        {
            unsafe
            {
                fixed (float* mPtr = &m[0])
                {
                    _glLoadMatrixf(mPtr);
                }
            }
        }

        public static void LoadName(int name) => _glLoadName((uint)name);

        public static void LoadTransposeMatrixf(float[] m)
        {
            unsafe
            {
                fixed (float* mPtr = &m[0])
                {
                    _glLoadTransposeMatrixf(mPtr);
                }
            }
        }

        public static void Map1f(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, float u1, float u2, int stride, int order, float[] points)
        {
            unsafe
            {
                fixed (float* pointsPtr = &points[0])
                {
                    _glMap1f(target, u1, u2, stride, order, pointsPtr);
                }
            }
        }

        public static void Map2f(SpiceEngine.GLFWBindings.GLEnums.MapTarget target, float u1, float u2, int ustride, int uorder, float v1, float v2, int vstride, int vorder, float[] points)
        {
            unsafe
            {
                fixed (float* pointsPtr = &points[0])
                {
                    _glMap2f(target, u1, u2, ustride, uorder, v1, v2, vstride, vorder, pointsPtr);
                }
            }
        }

        public static IntPtr MapBuffer(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, SpiceEngine.GLFWBindings.GLEnums.BufferAccessARB access) => new IntPtr(_glMapBuffer(target, access));

        public static IntPtr MapBufferRange(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, IntPtr offset, IntPtr length, SpiceEngine.GLFWBindings.GLEnums.MapBufferAccessMask access) => new IntPtr(_glMapBufferRange(target, offset, length, access));

        public static IntPtr MapNamedBuffer(int buffer, SpiceEngine.GLFWBindings.GLEnums.BufferAccessARB access) => new IntPtr(_glMapNamedBuffer((uint)buffer, access));

        public static IntPtr MapNamedBufferRange(int buffer, IntPtr offset, IntPtr length, SpiceEngine.GLFWBindings.GLEnums.MapBufferAccessMask access) => new IntPtr(_glMapNamedBufferRange((uint)buffer, offset, length, access));

        public static void Materialfv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glMaterialfv(face, pname, @paramsPtr);
                }
            }
        }

        public static void Materialiv(SpiceEngine.GLFWBindings.GLEnums.MaterialFace face, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glMaterialiv(face, pname, @paramsPtr);
                }
            }
        }

        public static void MultiDrawArrays(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int[] first, int[] count, int drawcount)
        {
            unsafe
            {
                fixed (int* firstPtr = &first[0])
                {
                    fixed (int* countPtr = &count[0])
                    {
                        _glMultiDrawArrays(mode, firstPtr, countPtr, drawcount);
                    }
                }
            }
        }

        public static void MultiDrawArraysIndirect(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, IntPtr indirect, int drawcount, int stride) => _glMultiDrawArraysIndirect(mode, indirect.ToPointer(), drawcount, stride);

        public static void MultiDrawArraysIndirectCount(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, IntPtr indirect, IntPtr drawcount, int maxdrawcount, int stride) => _glMultiDrawArraysIndirectCount(mode, indirect.ToPointer(), drawcount, maxdrawcount, stride);

        public static void MultiDrawElements(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int[] count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void** indices, int drawcount)
        {
            unsafe
            {
                fixed (int* countPtr = &count[0])
                {
                    _glMultiDrawElements(mode, countPtr, type, indices, drawcount);
                }
            }
        }

        public static void MultiDrawElementsBaseVertex(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, int[] count, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, void** indices, int drawcount, int[] basevertex)
        {
            unsafe
            {
                fixed (int* countPtr = &count[0])
                {
                    fixed (int* basevertexPtr = &basevertex[0])
                    {
                        _glMultiDrawElementsBaseVertex(mode, countPtr, type, indices, drawcount, basevertexPtr);
                    }
                }
            }
        }

        public static void MultiDrawElementsIndirect(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indirect, int drawcount, int stride) => _glMultiDrawElementsIndirect(mode, type, indirect.ToPointer(), drawcount, stride);

        public static void MultiDrawElementsIndirectCount(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType mode, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType type, IntPtr indirect, IntPtr drawcount, int maxdrawcount, int stride) => _glMultiDrawElementsIndirectCount(mode, type, indirect.ToPointer(), drawcount, maxdrawcount, stride);

        public static void MultiTexCoord1fv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glMultiTexCoord1fv(target, vPtr);
                }
            }
        }

        public static void MultiTexCoord1iv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glMultiTexCoord1iv(target, vPtr);
                }
            }
        }

        public static void MultiTexCoord2fv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glMultiTexCoord2fv(target, vPtr);
                }
            }
        }

        public static void MultiTexCoord2iv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glMultiTexCoord2iv(target, vPtr);
                }
            }
        }

        public static void MultiTexCoord3fv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glMultiTexCoord3fv(target, vPtr);
                }
            }
        }

        public static void MultiTexCoord3iv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glMultiTexCoord3iv(target, vPtr);
                }
            }
        }

        public static void MultiTexCoord4fv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glMultiTexCoord4fv(target, vPtr);
                }
            }
        }

        public static void MultiTexCoord4iv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit target, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glMultiTexCoord4iv(target, vPtr);
                }
            }
        }

        public static void MultiTexCoordP1ui(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int coords) => _glMultiTexCoordP1ui(texture, type, (uint)coords);

        public static void MultiTexCoordP1uiv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int[] coords)
        {
            unsafe
            {
                fixed (int* coordsPtr = &coords[0])
                {
                    _glMultiTexCoordP1uiv(texture, type, (uint*)coordsPtr);
                }
            }
        }

        public static void MultiTexCoordP2ui(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int coords) => _glMultiTexCoordP2ui(texture, type, (uint)coords);

        public static void MultiTexCoordP2uiv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int[] coords)
        {
            unsafe
            {
                fixed (int* coordsPtr = &coords[0])
                {
                    _glMultiTexCoordP2uiv(texture, type, (uint*)coordsPtr);
                }
            }
        }

        public static void MultiTexCoordP3ui(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int coords) => _glMultiTexCoordP3ui(texture, type, (uint)coords);

        public static void MultiTexCoordP3uiv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int[] coords)
        {
            unsafe
            {
                fixed (int* coordsPtr = &coords[0])
                {
                    _glMultiTexCoordP3uiv(texture, type, (uint*)coordsPtr);
                }
            }
        }

        public static void MultiTexCoordP4ui(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int coords) => _glMultiTexCoordP4ui(texture, type, (uint)coords);

        public static void MultiTexCoordP4uiv(SpiceEngine.GLFWBindings.GLEnums.TextureUnit texture, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int[] coords)
        {
            unsafe
            {
                fixed (int* coordsPtr = &coords[0])
                {
                    _glMultiTexCoordP4uiv(texture, type, (uint*)coordsPtr);
                }
            }
        }

        public static void MultMatrixf(float[] m)
        {
            unsafe
            {
                fixed (float* mPtr = &m[0])
                {
                    _glMultMatrixf(mPtr);
                }
            }
        }

        public static void MultTransposeMatrixf(float[] m)
        {
            unsafe
            {
                fixed (float* mPtr = &m[0])
                {
                    _glMultTransposeMatrixf(mPtr);
                }
            }
        }

        public static void NamedBufferData(int buffer, IntPtr size, IntPtr data, SpiceEngine.GLFWBindings.GLEnums.VertexBufferObjectUsage usage) => _glNamedBufferData((uint)buffer, size, data.ToPointer(), usage);

        public static void NamedBufferStorage(int buffer, IntPtr size, IntPtr data, SpiceEngine.GLFWBindings.GLEnums.BufferStorageMask flags) => _glNamedBufferStorage((uint)buffer, size, data.ToPointer(), flags);

        public static void NamedBufferSubData(int buffer, IntPtr offset, IntPtr size, IntPtr data) => _glNamedBufferSubData((uint)buffer, offset, size, data.ToPointer());

        public static void NamedFramebufferDrawBuffer(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.ColorBuffer buf) => _glNamedFramebufferDrawBuffer((uint)framebuffer, buf);

        public static void NamedFramebufferDrawBuffers(int framebuffer, int n, SpiceEngine.GLFWBindings.GLEnums.ColorBuffer[] bufs)
        {
            unsafe
            {
                fixed (SpiceEngine.GLFWBindings.GLEnums.ColorBuffer* bufsPtr = &bufs[0])
                {
                    _glNamedFramebufferDrawBuffers((uint)framebuffer, n, bufsPtr);
                }
            }
        }

        public static void NamedFramebufferParameteri(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferParameterName pname, int param) => _glNamedFramebufferParameteri((uint)framebuffer, pname, param);

        public static void NamedFramebufferReadBuffer(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.ColorBuffer src) => _glNamedFramebufferReadBuffer((uint)framebuffer, src);

        public static void NamedFramebufferRenderbuffer(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget renderbuffertarget, int renderbuffer) => _glNamedFramebufferRenderbuffer((uint)framebuffer, attachment, renderbuffertarget, (uint)renderbuffer);

        public static void NamedFramebufferTexture(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, int texture, int level) => _glNamedFramebufferTexture((uint)framebuffer, attachment, (uint)texture, level);

        public static void NamedFramebufferTextureLayer(int framebuffer, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment attachment, int texture, int level, int layer) => _glNamedFramebufferTextureLayer((uint)framebuffer, attachment, (uint)texture, level, layer);

        public static void NamedRenderbufferStorage(int renderbuffer, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height) => _glNamedRenderbufferStorage((uint)renderbuffer, internalformat, width, height);

        public static void NamedRenderbufferStorageMultisample(int renderbuffer, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height) => _glNamedRenderbufferStorageMultisample((uint)renderbuffer, samples, internalformat, width, height);

        public static void NewList(int list, SpiceEngine.GLFWBindings.GLEnums.ListMode mode) => _glNewList((uint)list, mode);

        public static void Normal3fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glNormal3fv(vPtr);
                }
            }
        }

        public static void Normal3iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glNormal3iv(vPtr);
                }
            }
        }

        public static void NormalP3ui(SpiceEngine.GLFWBindings.GLEnums.NormalPointerType type, int coords) => _glNormalP3ui(type, (uint)coords);

        public static void NormalP3uiv(SpiceEngine.GLFWBindings.GLEnums.NormalPointerType type, int[] coords)
        {
            unsafe
            {
                fixed (int* coordsPtr = &coords[0])
                {
                    _glNormalP3uiv(type, (uint*)coordsPtr);
                }
            }
        }

        public static void NormalPointer(SpiceEngine.GLFWBindings.GLEnums.NormalPointerType type, int stride, IntPtr pointer) => _glNormalPointer(type, stride, pointer.ToPointer());

        public static void ObjectLabel(SpiceEngine.GLFWBindings.GLEnums.ObjectIdentifier identifier, int name, int length, string label)
        {
            unsafe
            {
                var labelBytes = Encoding.UTF8.GetBytes(label);
                fixed (byte* labelPtr = &labelBytes[0])
                {
                    _glObjectLabel(identifier, (uint)name, length, (char*)labelPtr);
                }
            }
        }

        public static void ObjectPtrLabel(IntPtr ptr, int length, string label)
        {
            unsafe
            {
                var labelBytes = Encoding.UTF8.GetBytes(label);
                fixed (byte* labelPtr = &labelBytes[0])
                {
                    _glObjectPtrLabel(ptr.ToPointer(), length, (char*)labelPtr);
                }
            }
        }

        public static void PatchParameterfv(SpiceEngine.GLFWBindings.GLEnums.PatchParameterName pname, float[] values)
        {
            unsafe
            {
                fixed (float* valuesPtr = &values[0])
                {
                    _glPatchParameterfv(pname, valuesPtr);
                }
            }
        }

        public static void PixelMapfv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int mapsize, float[] values)
        {
            unsafe
            {
                fixed (float* valuesPtr = &values[0])
                {
                    _glPixelMapfv(map, mapsize, valuesPtr);
                }
            }
        }

        public static void PixelMapuiv(SpiceEngine.GLFWBindings.GLEnums.PixelMap map, int mapsize, int[] values)
        {
            unsafe
            {
                fixed (int* valuesPtr = &values[0])
                {
                    _glPixelMapuiv(map, mapsize, (uint*)valuesPtr);
                }
            }
        }

        public static void PointParameterfv(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glPointParameterfv(pname, @paramsPtr);
                }
            }
        }

        public static void PointParameteriv(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glPointParameteriv(pname, @paramsPtr);
                }
            }
        }

        public static void PrimitiveRestartIndex(int index) => _glPrimitiveRestartIndex((uint)index);

        public static void PrioritizeTexture(int texture, float priority)
        {
            var textures = new int[] { texture };
            var prioritys = new float[] { priority };
            PrioritizeTextures(1, textures, prioritys);
        }

        public static void PrioritizeTextures(int n, int[] textures, float[] priorities)
        {
            unsafe
            {
                fixed (int* texturesPtr = &textures[0])
                {
                    fixed (float* prioritiesPtr = &priorities[0])
                    {
                        _glPrioritizeTextures(n, (uint*)texturesPtr, prioritiesPtr);
                    }
                }
            }
        }

        public static void ProgramBinary(int program, int binaryFormat, IntPtr binary, int length) => _glProgramBinary((uint)program, binaryFormat, binary.ToPointer(), length);

        public static void ProgramParameteri(int program, SpiceEngine.GLFWBindings.GLEnums.ProgramParameterPName pname, int value) => _glProgramParameteri((uint)program, pname, value);

        public static void ProgramUniform1d(int program, int location, double v0) => _glProgramUniform1d((uint)program, location, v0);

        public static void ProgramUniform1dv(int program, int location, int count, double* value) => _glProgramUniform1dv((uint)program, location, count, value);

        public static void ProgramUniform1f(int program, int location, float v0) => _glProgramUniform1f((uint)program, location, v0);

        public static void ProgramUniform1fv(int program, int location, int count, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniform1fv((uint)program, location, count, valuePtr);
                }
            }
        }

        public static void ProgramUniform1i(int program, int location, int v0) => _glProgramUniform1i((uint)program, location, v0);

        public static void ProgramUniform1iv(int program, int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glProgramUniform1iv((uint)program, location, count, valuePtr);
                }
            }
        }

        public static void ProgramUniform1ui(int program, int location, int v0) => _glProgramUniform1ui((uint)program, location, (uint)v0);

        public static void ProgramUniform1uiv(int program, int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glProgramUniform1uiv((uint)program, location, count, (uint*)valuePtr);
                }
            }
        }

        public static void ProgramUniform2d(int program, int location, double v0, double v1) => _glProgramUniform2d((uint)program, location, v0, v1);

        public static void ProgramUniform2dv(int program, int location, int count, double* value) => _glProgramUniform2dv((uint)program, location, count, value);

        public static void ProgramUniform2f(int program, int location, float v0, float v1) => _glProgramUniform2f((uint)program, location, v0, v1);

        public static void ProgramUniform2fv(int program, int location, int count, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniform2fv((uint)program, location, count, valuePtr);
                }
            }
        }

        public static void ProgramUniform2i(int program, int location, int v0, int v1) => _glProgramUniform2i((uint)program, location, v0, v1);

        public static void ProgramUniform2iv(int program, int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glProgramUniform2iv((uint)program, location, count, valuePtr);
                }
            }
        }

        public static void ProgramUniform2ui(int program, int location, int v0, int v1) => _glProgramUniform2ui((uint)program, location, (uint)v0, (uint)v1);

        public static void ProgramUniform2uiv(int program, int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glProgramUniform2uiv((uint)program, location, count, (uint*)valuePtr);
                }
            }
        }

        public static void ProgramUniform3d(int program, int location, double v0, double v1, double v2) => _glProgramUniform3d((uint)program, location, v0, v1, v2);

        public static void ProgramUniform3dv(int program, int location, int count, double* value) => _glProgramUniform3dv((uint)program, location, count, value);

        public static void ProgramUniform3f(int program, int location, float v0, float v1, float v2) => _glProgramUniform3f((uint)program, location, v0, v1, v2);

        public static void ProgramUniform3fv(int program, int location, int count, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniform3fv((uint)program, location, count, valuePtr);
                }
            }
        }

        public static void ProgramUniform3i(int program, int location, int v0, int v1, int v2) => _glProgramUniform3i((uint)program, location, v0, v1, v2);

        public static void ProgramUniform3iv(int program, int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glProgramUniform3iv((uint)program, location, count, valuePtr);
                }
            }
        }

        public static void ProgramUniform3ui(int program, int location, int v0, int v1, int v2) => _glProgramUniform3ui((uint)program, location, (uint)v0, (uint)v1, (uint)v2);

        public static void ProgramUniform3uiv(int program, int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glProgramUniform3uiv((uint)program, location, count, (uint*)valuePtr);
                }
            }
        }

        public static void ProgramUniform4d(int program, int location, double v0, double v1, double v2, double v3) => _glProgramUniform4d((uint)program, location, v0, v1, v2, v3);

        public static void ProgramUniform4dv(int program, int location, int count, double* value) => _glProgramUniform4dv((uint)program, location, count, value);

        public static void ProgramUniform4f(int program, int location, float v0, float v1, float v2, float v3) => _glProgramUniform4f((uint)program, location, v0, v1, v2, v3);

        public static void ProgramUniform4fv(int program, int location, int count, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniform4fv((uint)program, location, count, valuePtr);
                }
            }
        }

        public static void ProgramUniform4i(int program, int location, int v0, int v1, int v2, int v3) => _glProgramUniform4i((uint)program, location, v0, v1, v2, v3);

        public static void ProgramUniform4iv(int program, int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glProgramUniform4iv((uint)program, location, count, valuePtr);
                }
            }
        }

        public static void ProgramUniform4ui(int program, int location, int v0, int v1, int v2, int v3) => _glProgramUniform4ui((uint)program, location, (uint)v0, (uint)v1, (uint)v2, (uint)v3);

        public static void ProgramUniform4uiv(int program, int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glProgramUniform4uiv((uint)program, location, count, (uint*)valuePtr);
                }
            }
        }

        public static void ProgramUniformMatrix2dv(int program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix2dv((uint)program, location, count, transpose, value);

        public static void ProgramUniformMatrix2fv(int program, int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniformMatrix2fv((uint)program, location, count, transpose, valuePtr);
                }
            }
        }

        public static void ProgramUniformMatrix2x3dv(int program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix2x3dv((uint)program, location, count, transpose, value);

        public static void ProgramUniformMatrix2x3fv(int program, int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniformMatrix2x3fv((uint)program, location, count, transpose, valuePtr);
                }
            }
        }

        public static void ProgramUniformMatrix2x4dv(int program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix2x4dv((uint)program, location, count, transpose, value);

        public static void ProgramUniformMatrix2x4fv(int program, int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniformMatrix2x4fv((uint)program, location, count, transpose, valuePtr);
                }
            }
        }

        public static void ProgramUniformMatrix3dv(int program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix3dv((uint)program, location, count, transpose, value);

        public static void ProgramUniformMatrix3fv(int program, int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniformMatrix3fv((uint)program, location, count, transpose, valuePtr);
                }
            }
        }

        public static void ProgramUniformMatrix3x2dv(int program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix3x2dv((uint)program, location, count, transpose, value);

        public static void ProgramUniformMatrix3x2fv(int program, int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniformMatrix3x2fv((uint)program, location, count, transpose, valuePtr);
                }
            }
        }

        public static void ProgramUniformMatrix3x4dv(int program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix3x4dv((uint)program, location, count, transpose, value);

        public static void ProgramUniformMatrix3x4fv(int program, int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniformMatrix3x4fv((uint)program, location, count, transpose, valuePtr);
                }
            }
        }

        public static void ProgramUniformMatrix4dv(int program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix4dv((uint)program, location, count, transpose, value);

        public static void ProgramUniformMatrix4fv(int program, int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniformMatrix4fv((uint)program, location, count, transpose, valuePtr);
                }
            }
        }

        public static void ProgramUniformMatrix4x2dv(int program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix4x2dv((uint)program, location, count, transpose, value);

        public static void ProgramUniformMatrix4x2fv(int program, int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniformMatrix4x2fv((uint)program, location, count, transpose, valuePtr);
                }
            }
        }

        public static void ProgramUniformMatrix4x3dv(int program, int location, int count, bool transpose, double* value) => _glProgramUniformMatrix4x3dv((uint)program, location, count, transpose, value);

        public static void ProgramUniformMatrix4x3fv(int program, int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glProgramUniformMatrix4x3fv((uint)program, location, count, transpose, valuePtr);
                }
            }
        }

        public static void PushDebugGroup(SpiceEngine.GLFWBindings.GLEnums.DebugSource source, int id, int length, string message)
        {
            unsafe
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                fixed (byte* messagePtr = &messageBytes[0])
                {
                    _glPushDebugGroup(source, (uint)id, length, (char*)messagePtr);
                }
            }
        }

        public static void PushName(int name) => _glPushName((uint)name);

        public static void QueryCounter(int id, SpiceEngine.GLFWBindings.GLEnums.QueryCounterTarget target) => _glQueryCounter((uint)id, target);

        public static void RasterPos2fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glRasterPos2fv(vPtr);
                }
            }
        }

        public static void RasterPos2iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glRasterPos2iv(vPtr);
                }
            }
        }

        public static void RasterPos3fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glRasterPos3fv(vPtr);
                }
            }
        }

        public static void RasterPos3iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glRasterPos3iv(vPtr);
                }
            }
        }

        public static void RasterPos4fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glRasterPos4fv(vPtr);
                }
            }
        }

        public static void RasterPos4iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glRasterPos4iv(vPtr);
                }
            }
        }

        public static void ReadnPixels(int x, int y, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, int bufSize, IntPtr data) => _glReadnPixels(x, y, width, height, format, type, bufSize, data.ToPointer());

        public static void ReadPixels(int x, int y, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glReadPixels(x, y, width, height, format, type, pixels.ToPointer());

        public static void Rectfv(float[] v1, float[] v2)
        {
            unsafe
            {
                fixed (float* v1Ptr = &v1[0])
                {
                    fixed (float* v2Ptr = &v2[0])
                    {
                        _glRectfv(v1Ptr, v2Ptr);
                    }
                }
            }
        }

        public static void Rectiv(int[] v1, int[] v2)
        {
            unsafe
            {
                fixed (int* v1Ptr = &v1[0])
                {
                    fixed (int* v2Ptr = &v2[0])
                    {
                        _glRectiv(v1Ptr, v2Ptr);
                    }
                }
            }
        }

        public static void SampleMaski(int maskNumber, int mask) => _glSampleMaski((uint)maskNumber, mask);

        public static void SamplerParameterf(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterF pname, float param) => _glSamplerParameterf((uint)sampler, pname, param);

        public static void SamplerParameterfv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterF pname, float[] param)
        {
            unsafe
            {
                fixed (float* paramPtr = &param[0])
                {
                    _glSamplerParameterfv((uint)sampler, pname, paramPtr);
                }
            }
        }

        public static void SamplerParameteri(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int param) => _glSamplerParameteri((uint)sampler, pname, param);

        public static void SamplerParameterIiv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int[] param)
        {
            unsafe
            {
                fixed (int* paramPtr = &param[0])
                {
                    _glSamplerParameterIiv((uint)sampler, pname, paramPtr);
                }
            }
        }

        public static void SamplerParameterIuiv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int[] param)
        {
            unsafe
            {
                fixed (int* paramPtr = &param[0])
                {
                    _glSamplerParameterIuiv((uint)sampler, pname, (uint*)paramPtr);
                }
            }
        }

        public static void SamplerParameteriv(int sampler, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI pname, int[] param)
        {
            unsafe
            {
                fixed (int* paramPtr = &param[0])
                {
                    _glSamplerParameteriv((uint)sampler, pname, paramPtr);
                }
            }
        }

        public static void ScissorArrayv(int first, int count, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glScissorArrayv((uint)first, count, vPtr);
                }
            }
        }

        public static void ScissorIndexed(int index, int left, int bottom, int width, int height) => _glScissorIndexed((uint)index, left, bottom, width, height);

        public static void ScissorIndexedv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glScissorIndexedv((uint)index, vPtr);
                }
            }
        }

        public static void SecondaryColor3fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glSecondaryColor3fv(vPtr);
                }
            }
        }

        public static void SecondaryColor3iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glSecondaryColor3iv(vPtr);
                }
            }
        }

        public static void SecondaryColor3ui(int red, int green, int blue) => _glSecondaryColor3ui((uint)red, (uint)green, (uint)blue);

        public static void SecondaryColor3uiv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glSecondaryColor3uiv((uint*)vPtr);
                }
            }
        }

        public static void SecondaryColorP3ui(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int color) => _glSecondaryColorP3ui(type, (uint)color);

        public static void SecondaryColorP3uiv(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int[] color)
        {
            unsafe
            {
                fixed (int* colorPtr = &color[0])
                {
                    _glSecondaryColorP3uiv(type, (uint*)colorPtr);
                }
            }
        }

        public static void SecondaryColorPointer(int size, SpiceEngine.GLFWBindings.GLEnums.ColorPointerType type, int stride, IntPtr pointer) => _glSecondaryColorPointer(size, type, stride, pointer.ToPointer());

        public static void SelectBuffer(int size, int[] buffer)
        {
            unsafe
            {
                fixed (int* bufferPtr = &buffer[0])
                {
                    _glSelectBuffer(size, (uint*)bufferPtr);
                }
            }
        }

        public static void ShaderBinary(int count, int[] shaders, SpiceEngine.GLFWBindings.GLEnums.ShaderBinaryFormat binaryFormat, IntPtr binary, int length)
        {
            unsafe
            {
                fixed (int* shadersPtr = &shaders[0])
                {
                    _glShaderBinary(count, (uint*)shadersPtr, binaryFormat, binary.ToPointer(), length);
                }
            }
        }

        public static void ShaderSource(int shader, int count, string[] @string, int[] length)
        {
            unsafe
            {
                var ptrs = new List<IntPtr>();
                var size = Marshal.SizeOf(typeof(IntPtr));
                var @stringPtr = Marshal.AllocHGlobal(size * @string.Length);
                
                for (var i = 0; i < @string.Length; i++)
                {
                    var @stringSinglePtr = Marshal.StringToHGlobalAnsi(@string[i]);
                    ptrs.Add(@stringSinglePtr);
                    Marshal.WriteIntPtr(@stringPtr, i * size, @stringSinglePtr);
                }
                
                fixed (int* lengthPtr = &length[0])
                {
                    _glShaderSource((uint)shader, count, (char**)@stringPtr, lengthPtr);
                }
                
                Marshal.FreeHGlobal(@stringPtr);
                
                foreach (var ptr in ptrs)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        public static void ShaderStorageBlockBinding(int program, int storageBlockIndex, int storageBlockBinding) => _glShaderStorageBlockBinding((uint)program, (uint)storageBlockIndex, (uint)storageBlockBinding);

        public static void SpecializeShader(int shader, string pEntryPoint, int numSpecializationConstants, int[] pConstantIndex, int[] pConstantValue)
        {
            unsafe
            {
                var pEntryPointBytes = Encoding.UTF8.GetBytes(pEntryPoint);
                fixed (byte* pEntryPointPtr = &pEntryPointBytes[0])
                {
                    fixed (int* pConstantIndexPtr = &pConstantIndex[0])
                    {
                        fixed (int* pConstantValuePtr = &pConstantValue[0])
                        {
                            _glSpecializeShader((uint)shader, (char*)pEntryPointPtr, (uint)numSpecializationConstants, (uint*)pConstantIndexPtr, (uint*)pConstantValuePtr);
                        }
                    }
                }
            }
        }

        public static void StencilFunc(SpiceEngine.GLFWBindings.GLEnums.StencilFunction func, int @ref, int mask) => _glStencilFunc(func, @ref, (uint)mask);

        public static void StencilFuncSeparate(SpiceEngine.GLFWBindings.GLEnums.StencilFaceDirection face, SpiceEngine.GLFWBindings.GLEnums.StencilFunction func, int @ref, int mask) => _glStencilFuncSeparate(face, func, @ref, (uint)mask);

        public static void StencilMask(int mask) => _glStencilMask((uint)mask);

        public static void StencilMaskSeparate(SpiceEngine.GLFWBindings.GLEnums.StencilFaceDirection face, int mask) => _glStencilMaskSeparate(face, (uint)mask);

        public static void TexBuffer(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int buffer) => _glTexBuffer(target, internalformat, (uint)buffer);

        public static void TexBufferRange(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int buffer, IntPtr offset, IntPtr size) => _glTexBufferRange(target, internalformat, (uint)buffer, offset, size);

        public static void TexCoord1fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glTexCoord1fv(vPtr);
                }
            }
        }

        public static void TexCoord1iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glTexCoord1iv(vPtr);
                }
            }
        }

        public static void TexCoord2fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glTexCoord2fv(vPtr);
                }
            }
        }

        public static void TexCoord2iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glTexCoord2iv(vPtr);
                }
            }
        }

        public static void TexCoord3fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glTexCoord3fv(vPtr);
                }
            }
        }

        public static void TexCoord3iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glTexCoord3iv(vPtr);
                }
            }
        }

        public static void TexCoord4fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glTexCoord4fv(vPtr);
                }
            }
        }

        public static void TexCoord4iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glTexCoord4iv(vPtr);
                }
            }
        }

        public static void TexCoordP1ui(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int coords) => _glTexCoordP1ui(type, (uint)coords);

        public static void TexCoordP1uiv(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int[] coords)
        {
            unsafe
            {
                fixed (int* coordsPtr = &coords[0])
                {
                    _glTexCoordP1uiv(type, (uint*)coordsPtr);
                }
            }
        }

        public static void TexCoordP2ui(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int coords) => _glTexCoordP2ui(type, (uint)coords);

        public static void TexCoordP2uiv(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int[] coords)
        {
            unsafe
            {
                fixed (int* coordsPtr = &coords[0])
                {
                    _glTexCoordP2uiv(type, (uint*)coordsPtr);
                }
            }
        }

        public static void TexCoordP3ui(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int coords) => _glTexCoordP3ui(type, (uint)coords);

        public static void TexCoordP3uiv(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int[] coords)
        {
            unsafe
            {
                fixed (int* coordsPtr = &coords[0])
                {
                    _glTexCoordP3uiv(type, (uint*)coordsPtr);
                }
            }
        }

        public static void TexCoordP4ui(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int coords) => _glTexCoordP4ui(type, (uint)coords);

        public static void TexCoordP4uiv(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int[] coords)
        {
            unsafe
            {
                fixed (int* coordsPtr = &coords[0])
                {
                    _glTexCoordP4uiv(type, (uint*)coordsPtr);
                }
            }
        }

        public static void TexCoordPointer(int size, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType type, int stride, IntPtr pointer) => _glTexCoordPointer(size, type, stride, pointer.ToPointer());

        public static void TexEnvfv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glTexEnvfv(target, pname, @paramsPtr);
                }
            }
        }

        public static void TexEnviv(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glTexEnviv(target, pname, @paramsPtr);
                }
            }
        }

        public static void TexGenfv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glTexGenfv(coord, pname, @paramsPtr);
                }
            }
        }

        public static void TexGeniv(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName coord, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glTexGeniv(coord, pname, @paramsPtr);
                }
            }
        }

        public static void TexImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int border, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glTexImage1D(target, level, internalformat, width, border, format, type, pixels.ToPointer());

        public static void TexImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int border, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels.ToPointer());

        public static void TexImage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth, int border, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glTexImage3D(target, level, internalformat, width, height, depth, border, format, type, pixels.ToPointer());

        public static void TexParameterfv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, float[] @params)
        {
            unsafe
            {
                fixed (float* @paramsPtr = &@params[0])
                {
                    _glTexParameterfv(target, pname, @paramsPtr);
                }
            }
        }

        public static void TexParameterIiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glTexParameterIiv(target, pname, @paramsPtr);
                }
            }
        }

        public static void TexParameterIuiv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glTexParameterIuiv(target, pname, (uint*)@paramsPtr);
                }
            }
        }

        public static void TexParameteriv(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glTexParameteriv(target, pname, @paramsPtr);
                }
            }
        }

        public static void TexSubImage1D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int width, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glTexSubImage1D(target, level, xoffset, width, format, type, pixels.ToPointer());

        public static void TexSubImage2D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels.ToPointer());

        public static void TexSubImage3D(SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glTexSubImage3D(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels.ToPointer());

        public static void TextureBuffer(int texture, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int buffer) => _glTextureBuffer((uint)texture, internalformat, (uint)buffer);

        public static void TextureBufferRange(int texture, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int buffer, IntPtr offset, IntPtr size) => _glTextureBufferRange((uint)texture, internalformat, (uint)buffer, offset, size);

        public static void TextureParameterf(int texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, float param) => _glTextureParameterf((uint)texture, pname, param);

        public static void TextureParameterfv(int texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, float[] param)
        {
            unsafe
            {
                fixed (float* paramPtr = &param[0])
                {
                    _glTextureParameterfv((uint)texture, pname, paramPtr);
                }
            }
        }

        public static void TextureParameteri(int texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int param) => _glTextureParameteri((uint)texture, pname, param);

        public static void TextureParameterIiv(int texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glTextureParameterIiv((uint)texture, pname, @paramsPtr);
                }
            }
        }

        public static void TextureParameterIuiv(int texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int[] @params)
        {
            unsafe
            {
                fixed (int* @paramsPtr = &@params[0])
                {
                    _glTextureParameterIuiv((uint)texture, pname, (uint*)@paramsPtr);
                }
            }
        }

        public static void TextureParameteriv(int texture, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName pname, int[] param)
        {
            unsafe
            {
                fixed (int* paramPtr = &param[0])
                {
                    _glTextureParameteriv((uint)texture, pname, paramPtr);
                }
            }
        }

        public static void TextureStorage1D(int texture, int levels, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width) => _glTextureStorage1D((uint)texture, levels, internalformat, width);

        public static void TextureStorage2D(int texture, int levels, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height) => _glTextureStorage2D((uint)texture, levels, internalformat, width, height);

        public static void TextureStorage2DMultisample(int texture, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, bool fixedsamplelocations) => _glTextureStorage2DMultisample((uint)texture, samples, internalformat, width, height, fixedsamplelocations);

        public static void TextureStorage3D(int texture, int levels, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth) => _glTextureStorage3D((uint)texture, levels, internalformat, width, height, depth);

        public static void TextureStorage3DMultisample(int texture, int samples, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTextureStorage3DMultisample((uint)texture, samples, internalformat, width, height, depth, fixedsamplelocations);

        public static void TextureSubImage1D(int texture, int level, int xoffset, int width, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glTextureSubImage1D((uint)texture, level, xoffset, width, format, type, pixels.ToPointer());

        public static void TextureSubImage2D(int texture, int level, int xoffset, int yoffset, int width, int height, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glTextureSubImage2D((uint)texture, level, xoffset, yoffset, width, height, format, type, pixels.ToPointer());

        public static void TextureSubImage3D(int texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, SpiceEngine.GLFWBindings.GLEnums.PixelFormat format, SpiceEngine.GLFWBindings.GLEnums.PixelType type, IntPtr pixels) => _glTextureSubImage3D((uint)texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels.ToPointer());

        public static void TextureView(int texture, SpiceEngine.GLFWBindings.GLEnums.TextureTarget target, int origtexture, SpiceEngine.GLFWBindings.GLEnums.InternalFormat internalformat, int minlevel, int numlevels, int minlayer, int numlayers) => _glTextureView((uint)texture, target, (uint)origtexture, internalformat, (uint)minlevel, (uint)numlevels, (uint)minlayer, (uint)numlayers);

        public static void TransformFeedbackBufferBase(int xfb, int index, int buffer) => _glTransformFeedbackBufferBase((uint)xfb, (uint)index, (uint)buffer);

        public static void TransformFeedbackBufferRange(int xfb, int index, int buffer, IntPtr offset, IntPtr size) => _glTransformFeedbackBufferRange((uint)xfb, (uint)index, (uint)buffer, offset, size);

        public static void TransformFeedbackVaryings(int program, int count, string[] varyings, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackBufferMode bufferMode)
        {
            var ptrs = new List<IntPtr>();
            var size = Marshal.SizeOf(typeof(IntPtr));
            var varyingsPtr = Marshal.AllocHGlobal(size * varyings.Length);
            
            for (var i = 0; i < varyings.Length; i++)
            {
                var varyingsSinglePtr = Marshal.StringToHGlobalAnsi(varyings[i]);
                ptrs.Add(varyingsSinglePtr);
                Marshal.WriteIntPtr(varyingsPtr, i * size, varyingsSinglePtr);
            }
            
            _glTransformFeedbackVaryings((uint)program, count, (char**)varyingsPtr, bufferMode);
            
            Marshal.FreeHGlobal(varyingsPtr);
            
            foreach (var ptr in ptrs)
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static void Uniform1fv(int location, int count, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniform1fv(location, count, valuePtr);
                }
            }
        }

        public static void Uniform1iv(int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glUniform1iv(location, count, valuePtr);
                }
            }
        }

        public static void Uniform1ui(int location, int v0) => _glUniform1ui(location, (uint)v0);

        public static void Uniform1uiv(int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glUniform1uiv(location, count, (uint*)valuePtr);
                }
            }
        }

        public static void Uniform2fv(int location, int count, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniform2fv(location, count, valuePtr);
                }
            }
        }

        public static void Uniform2iv(int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glUniform2iv(location, count, valuePtr);
                }
            }
        }

        public static void Uniform2ui(int location, int v0, int v1) => _glUniform2ui(location, (uint)v0, (uint)v1);

        public static void Uniform2uiv(int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glUniform2uiv(location, count, (uint*)valuePtr);
                }
            }
        }

        public static void Uniform3fv(int location, int count, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniform3fv(location, count, valuePtr);
                }
            }
        }

        public static void Uniform3iv(int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glUniform3iv(location, count, valuePtr);
                }
            }
        }

        public static void Uniform3ui(int location, int v0, int v1, int v2) => _glUniform3ui(location, (uint)v0, (uint)v1, (uint)v2);

        public static void Uniform3uiv(int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glUniform3uiv(location, count, (uint*)valuePtr);
                }
            }
        }

        public static void Uniform4fv(int location, int count, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniform4fv(location, count, valuePtr);
                }
            }
        }

        public static void Uniform4iv(int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glUniform4iv(location, count, valuePtr);
                }
            }
        }

        public static void Uniform4ui(int location, int v0, int v1, int v2, int v3) => _glUniform4ui(location, (uint)v0, (uint)v1, (uint)v2, (uint)v3);

        public static void Uniform4uiv(int location, int count, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glUniform4uiv(location, count, (uint*)valuePtr);
                }
            }
        }

        public static void UniformBlockBinding(int program, int uniformBlockIndex, int uniformBlockBinding) => _glUniformBlockBinding((uint)program, (uint)uniformBlockIndex, (uint)uniformBlockBinding);

        public static void UniformMatrix2fv(int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniformMatrix2fv(location, count, transpose, valuePtr);
                }
            }
        }

        public static void UniformMatrix2x3fv(int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniformMatrix2x3fv(location, count, transpose, valuePtr);
                }
            }
        }

        public static void UniformMatrix2x4fv(int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniformMatrix2x4fv(location, count, transpose, valuePtr);
                }
            }
        }

        public static void UniformMatrix3fv(int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniformMatrix3fv(location, count, transpose, valuePtr);
                }
            }
        }

        public static void UniformMatrix3x2fv(int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniformMatrix3x2fv(location, count, transpose, valuePtr);
                }
            }
        }

        public static void UniformMatrix3x4fv(int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniformMatrix3x4fv(location, count, transpose, valuePtr);
                }
            }
        }

        public static void UniformMatrix4fv(int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniformMatrix4fv(location, count, transpose, valuePtr);
                }
            }
        }

        public static void UniformMatrix4x2fv(int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniformMatrix4x2fv(location, count, transpose, valuePtr);
                }
            }
        }

        public static void UniformMatrix4x3fv(int location, int count, bool transpose, float[] value)
        {
            unsafe
            {
                fixed (float* valuePtr = &value[0])
                {
                    _glUniformMatrix4x3fv(location, count, transpose, valuePtr);
                }
            }
        }

        public static void UniformSubroutinesuiv(SpiceEngine.GLFWBindings.GLEnums.ShaderType shadertype, int count, int[] indices)
        {
            unsafe
            {
                fixed (int* indicesPtr = &indices[0])
                {
                    _glUniformSubroutinesuiv(shadertype, count, (uint*)indicesPtr);
                }
            }
        }

        public static bool UnmapNamedBuffer(int buffer) => _glUnmapNamedBuffer((uint)buffer);

        public static void UseProgram(int program) => _glUseProgram((uint)program);

        public static void UseProgramStages(int pipeline, SpiceEngine.GLFWBindings.GLEnums.UseProgramStageMask stages, int program) => _glUseProgramStages((uint)pipeline, stages, (uint)program);

        public static void ValidateProgram(int program) => _glValidateProgram((uint)program);

        public static void ValidateProgramPipeline(int pipeline) => _glValidateProgramPipeline((uint)pipeline);

        public static void Vertex2fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glVertex2fv(vPtr);
                }
            }
        }

        public static void Vertex2iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertex2iv(vPtr);
                }
            }
        }

        public static void Vertex3fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glVertex3fv(vPtr);
                }
            }
        }

        public static void Vertex3iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertex3iv(vPtr);
                }
            }
        }

        public static void Vertex4fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glVertex4fv(vPtr);
                }
            }
        }

        public static void Vertex4iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertex4iv(vPtr);
                }
            }
        }

        public static void VertexArrayAttribBinding(int vaobj, int attribindex, int bindingindex) => _glVertexArrayAttribBinding((uint)vaobj, (uint)attribindex, (uint)bindingindex);

        public static void VertexArrayAttribFormat(int vaobj, int attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribType type, bool normalized, int relativeoffset) => _glVertexArrayAttribFormat((uint)vaobj, (uint)attribindex, size, type, normalized, (uint)relativeoffset);

        public static void VertexArrayAttribIFormat(int vaobj, int attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribIType type, int relativeoffset) => _glVertexArrayAttribIFormat((uint)vaobj, (uint)attribindex, size, type, (uint)relativeoffset);

        public static void VertexArrayAttribLFormat(int vaobj, int attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribLType type, int relativeoffset) => _glVertexArrayAttribLFormat((uint)vaobj, (uint)attribindex, size, type, (uint)relativeoffset);

        public static void VertexArrayBindingDivisor(int vaobj, int bindingindex, int divisor) => _glVertexArrayBindingDivisor((uint)vaobj, (uint)bindingindex, (uint)divisor);

        public static void VertexArrayElementBuffer(int vaobj, int buffer) => _glVertexArrayElementBuffer((uint)vaobj, (uint)buffer);

        public static void VertexArrayVertexBuffer(int vaobj, int bindingindex, int buffer, IntPtr offset, int stride) => _glVertexArrayVertexBuffer((uint)vaobj, (uint)bindingindex, (uint)buffer, offset, stride);

        public static void VertexArrayVertexBuffers(int vaobj, int first, int count, int[] buffers, IntPtr* offsets, int[] strides)
        {
            unsafe
            {
                fixed (int* buffersPtr = &buffers[0])
                {
                    fixed (int* stridesPtr = &strides[0])
                    {
                        _glVertexArrayVertexBuffers((uint)vaobj, (uint)first, count, (uint*)buffersPtr, offsets, stridesPtr);
                    }
                }
            }
        }

        public static void VertexAttrib1d(int index, double x) => _glVertexAttrib1d((uint)index, x);

        public static void VertexAttrib1dv(int index, double* v) => _glVertexAttrib1dv((uint)index, v);

        public static void VertexAttrib1f(int index, float x) => _glVertexAttrib1f((uint)index, x);

        public static void VertexAttrib1fv(int index, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glVertexAttrib1fv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttrib1s(int index, short x) => _glVertexAttrib1s((uint)index, x);

        public static void VertexAttrib1sv(int index, short* v) => _glVertexAttrib1sv((uint)index, v);

        public static void VertexAttrib2d(int index, double x, double y) => _glVertexAttrib2d((uint)index, x, y);

        public static void VertexAttrib2dv(int index, double* v) => _glVertexAttrib2dv((uint)index, v);

        public static void VertexAttrib2f(int index, float x, float y) => _glVertexAttrib2f((uint)index, x, y);

        public static void VertexAttrib2fv(int index, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glVertexAttrib2fv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttrib2s(int index, short x, short y) => _glVertexAttrib2s((uint)index, x, y);

        public static void VertexAttrib2sv(int index, short* v) => _glVertexAttrib2sv((uint)index, v);

        public static void VertexAttrib3d(int index, double x, double y, double z) => _glVertexAttrib3d((uint)index, x, y, z);

        public static void VertexAttrib3dv(int index, double* v) => _glVertexAttrib3dv((uint)index, v);

        public static void VertexAttrib3f(int index, float x, float y, float z) => _glVertexAttrib3f((uint)index, x, y, z);

        public static void VertexAttrib3fv(int index, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glVertexAttrib3fv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttrib3s(int index, short x, short y, short z) => _glVertexAttrib3s((uint)index, x, y, z);

        public static void VertexAttrib3sv(int index, short* v) => _glVertexAttrib3sv((uint)index, v);

        public static void VertexAttrib4bv(int index, byte* v) => _glVertexAttrib4bv((uint)index, v);

        public static void VertexAttrib4d(int index, double x, double y, double z, double w) => _glVertexAttrib4d((uint)index, x, y, z, w);

        public static void VertexAttrib4dv(int index, double* v) => _glVertexAttrib4dv((uint)index, v);

        public static void VertexAttrib4f(int index, float x, float y, float z, float w) => _glVertexAttrib4f((uint)index, x, y, z, w);

        public static void VertexAttrib4fv(int index, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glVertexAttrib4fv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttrib4iv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttrib4iv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttrib4Nbv(int index, byte* v) => _glVertexAttrib4Nbv((uint)index, v);

        public static void VertexAttrib4Niv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttrib4Niv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttrib4Nsv(int index, short* v) => _glVertexAttrib4Nsv((uint)index, v);

        public static void VertexAttrib4Nub(int index, byte x, byte y, byte z, byte w) => _glVertexAttrib4Nub((uint)index, x, y, z, w);

        public static void VertexAttrib4Nubv(int index, byte* v) => _glVertexAttrib4Nubv((uint)index, v);

        public static void VertexAttrib4Nuiv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttrib4Nuiv((uint)index, (uint*)vPtr);
                }
            }
        }

        public static void VertexAttrib4Nusv(int index, ushort* v) => _glVertexAttrib4Nusv((uint)index, v);

        public static void VertexAttrib4s(int index, short x, short y, short z, short w) => _glVertexAttrib4s((uint)index, x, y, z, w);

        public static void VertexAttrib4sv(int index, short* v) => _glVertexAttrib4sv((uint)index, v);

        public static void VertexAttrib4ubv(int index, byte* v) => _glVertexAttrib4ubv((uint)index, v);

        public static void VertexAttrib4uiv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttrib4uiv((uint)index, (uint*)vPtr);
                }
            }
        }

        public static void VertexAttrib4usv(int index, ushort* v) => _glVertexAttrib4usv((uint)index, v);

        public static void VertexAttribBinding(int attribindex, int bindingindex) => _glVertexAttribBinding((uint)attribindex, (uint)bindingindex);

        public static void VertexAttribDivisor(int index, int divisor) => _glVertexAttribDivisor((uint)index, (uint)divisor);

        public static void VertexAttribFormat(int attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribType type, bool normalized, int relativeoffset) => _glVertexAttribFormat((uint)attribindex, size, type, normalized, (uint)relativeoffset);

        public static void VertexAttribI1i(int index, int x) => _glVertexAttribI1i((uint)index, x);

        public static void VertexAttribI1iv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttribI1iv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttribI1ui(int index, int x) => _glVertexAttribI1ui((uint)index, (uint)x);

        public static void VertexAttribI1uiv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttribI1uiv((uint)index, (uint*)vPtr);
                }
            }
        }

        public static void VertexAttribI2i(int index, int x, int y) => _glVertexAttribI2i((uint)index, x, y);

        public static void VertexAttribI2iv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttribI2iv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttribI2ui(int index, int x, int y) => _glVertexAttribI2ui((uint)index, (uint)x, (uint)y);

        public static void VertexAttribI2uiv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttribI2uiv((uint)index, (uint*)vPtr);
                }
            }
        }

        public static void VertexAttribI3i(int index, int x, int y, int z) => _glVertexAttribI3i((uint)index, x, y, z);

        public static void VertexAttribI3iv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttribI3iv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttribI3ui(int index, int x, int y, int z) => _glVertexAttribI3ui((uint)index, (uint)x, (uint)y, (uint)z);

        public static void VertexAttribI3uiv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttribI3uiv((uint)index, (uint*)vPtr);
                }
            }
        }

        public static void VertexAttribI4bv(int index, byte* v) => _glVertexAttribI4bv((uint)index, v);

        public static void VertexAttribI4i(int index, int x, int y, int z, int w) => _glVertexAttribI4i((uint)index, x, y, z, w);

        public static void VertexAttribI4iv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttribI4iv((uint)index, vPtr);
                }
            }
        }

        public static void VertexAttribI4sv(int index, short* v) => _glVertexAttribI4sv((uint)index, v);

        public static void VertexAttribI4ubv(int index, byte* v) => _glVertexAttribI4ubv((uint)index, v);

        public static void VertexAttribI4ui(int index, int x, int y, int z, int w) => _glVertexAttribI4ui((uint)index, (uint)x, (uint)y, (uint)z, (uint)w);

        public static void VertexAttribI4uiv(int index, int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glVertexAttribI4uiv((uint)index, (uint*)vPtr);
                }
            }
        }

        public static void VertexAttribI4usv(int index, ushort* v) => _glVertexAttribI4usv((uint)index, v);

        public static void VertexAttribIFormat(int attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribIType type, int relativeoffset) => _glVertexAttribIFormat((uint)attribindex, size, type, (uint)relativeoffset);

        public static void VertexAttribIPointer(int index, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribIType type, int stride, IntPtr pointer) => _glVertexAttribIPointer((uint)index, size, type, stride, pointer.ToPointer());

        public static void VertexAttribL1d(int index, double x) => _glVertexAttribL1d((uint)index, x);

        public static void VertexAttribL1dv(int index, double* v) => _glVertexAttribL1dv((uint)index, v);

        public static void VertexAttribL2d(int index, double x, double y) => _glVertexAttribL2d((uint)index, x, y);

        public static void VertexAttribL2dv(int index, double* v) => _glVertexAttribL2dv((uint)index, v);

        public static void VertexAttribL3d(int index, double x, double y, double z) => _glVertexAttribL3d((uint)index, x, y, z);

        public static void VertexAttribL3dv(int index, double* v) => _glVertexAttribL3dv((uint)index, v);

        public static void VertexAttribL4d(int index, double x, double y, double z, double w) => _glVertexAttribL4d((uint)index, x, y, z, w);

        public static void VertexAttribL4dv(int index, double* v) => _glVertexAttribL4dv((uint)index, v);

        public static void VertexAttribLFormat(int attribindex, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribLType type, int relativeoffset) => _glVertexAttribLFormat((uint)attribindex, size, type, (uint)relativeoffset);

        public static void VertexAttribLPointer(int index, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribLType type, int stride, IntPtr pointer) => _glVertexAttribLPointer((uint)index, size, type, stride, pointer.ToPointer());

        public static void VertexAttribP1ui(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int value) => _glVertexAttribP1ui((uint)index, type, normalized, (uint)value);

        public static void VertexAttribP1uiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glVertexAttribP1uiv((uint)index, type, normalized, (uint*)valuePtr);
                }
            }
        }

        public static void VertexAttribP2ui(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int value) => _glVertexAttribP2ui((uint)index, type, normalized, (uint)value);

        public static void VertexAttribP2uiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glVertexAttribP2uiv((uint)index, type, normalized, (uint*)valuePtr);
                }
            }
        }

        public static void VertexAttribP3ui(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int value) => _glVertexAttribP3ui((uint)index, type, normalized, (uint)value);

        public static void VertexAttribP3uiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glVertexAttribP3uiv((uint)index, type, normalized, (uint*)valuePtr);
                }
            }
        }

        public static void VertexAttribP4ui(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int value) => _glVertexAttribP4ui((uint)index, type, normalized, (uint)value);

        public static void VertexAttribP4uiv(int index, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glVertexAttribP4uiv((uint)index, type, normalized, (uint*)valuePtr);
                }
            }
        }

        public static void VertexAttribPointer(int index, int size, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType type, bool normalized, int stride, IntPtr pointer) => _glVertexAttribPointer((uint)index, size, type, normalized, stride, pointer.ToPointer());

        public static void VertexBindingDivisor(int bindingindex, int divisor) => _glVertexBindingDivisor((uint)bindingindex, (uint)divisor);

        public static void VertexP2ui(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, int value) => _glVertexP2ui(type, (uint)value);

        public static void VertexP2uiv(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glVertexP2uiv(type, (uint*)valuePtr);
                }
            }
        }

        public static void VertexP3ui(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, int value) => _glVertexP3ui(type, (uint)value);

        public static void VertexP3uiv(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glVertexP3uiv(type, (uint*)valuePtr);
                }
            }
        }

        public static void VertexP4ui(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, int value) => _glVertexP4ui(type, (uint)value);

        public static void VertexP4uiv(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, int[] value)
        {
            unsafe
            {
                fixed (int* valuePtr = &value[0])
                {
                    _glVertexP4uiv(type, (uint*)valuePtr);
                }
            }
        }

        public static void VertexPointer(int size, SpiceEngine.GLFWBindings.GLEnums.VertexPointerType type, int stride, IntPtr pointer) => _glVertexPointer(size, type, stride, pointer.ToPointer());

        public static void ViewportArrayv(int first, int count, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glViewportArrayv((uint)first, count, vPtr);
                }
            }
        }

        public static void ViewportIndexedf(int index, float x, float y, float w, float h) => _glViewportIndexedf((uint)index, x, y, w, h);

        public static void ViewportIndexedfv(int index, float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glViewportIndexedfv((uint)index, vPtr);
                }
            }
        }

        public static void WindowPos2fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glWindowPos2fv(vPtr);
                }
            }
        }

        public static void WindowPos2iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glWindowPos2iv(vPtr);
                }
            }
        }

        public static void WindowPos3fv(float[] v)
        {
            unsafe
            {
                fixed (float* vPtr = &v[0])
                {
                    _glWindowPos3fv(vPtr);
                }
            }
        }

        public static void WindowPos3iv(int[] v)
        {
            unsafe
            {
                fixed (int* vPtr = &v[0])
                {
                    _glWindowPos3iv(vPtr);
                }
            }
        }

        public static void ClearColor(Color4 color) => ClearColor(color.R, color.G, color.B, color.A);

        public static Color4 ReadPixels(int x, int y, int width, int height, GLFWBindings.GLEnums.PixelFormat format, GLFWBindings.GLEnums.PixelType type)
        {
            var bytes = new byte[4];

            unsafe
            {
                fixed (byte* bytesPtr = &bytes[0])
                {
                    ReadPixels(x, y, width, height, format, type, (IntPtr)bytesPtr);
                    return new Color4((int)bytes[0], (int)bytes[1], (int)bytes[2], (int)bytes[3]);
                }
            }
        }

        public static void BufferData<T>(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB target, int size, T[] data, SpiceEngine.GLFWBindings.GLEnums.BufferUsageARB usage) where T : struct
        {
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                BufferData(target, (IntPtr)size, handle.AddrOfPinnedObject(), usage);
            }
            finally
            {
                handle.Free();
            }
        }

        private static T GetFunctionDelegate<T>(string name) where T : Delegate
        {
            var address = GLFW.GetProcAddress(name);
            return Marshal.GetDelegateForFunctionPointer<T>(address);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool DEL_B_EBufferTargetARBE(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool DEL_B_EEnableCapE(SpiceEngine.GLFWBindings.GLEnums.EnableCap v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool DEL_B_EEnableCapEUi(SpiceEngine.GLFWBindings.GLEnums.EnableCap v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool DEL_B_IUipBp(int v0, uint* v1, bool* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool DEL_B_StpSyncStp(SpiceEngine.GLFWBindings.GLStructs.Sync* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool DEL_B_Ui(uint v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate byte* DEL_Byp_EStringNameE(SpiceEngine.GLFWBindings.GLEnums.StringName v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate byte* DEL_Byp_EStringNameEUi(SpiceEngine.GLFWBindings.GLEnums.StringName v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate SpiceEngine.GLFWBindings.GLEnums.ErrorCode DEL_EErrorCodeE_();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate SpiceEngine.GLFWBindings.GLEnums.FramebufferStatus DEL_EFramebufferStatusE_EFramebufferTargetE(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate SpiceEngine.GLFWBindings.GLEnums.FramebufferStatus DEL_EFramebufferStatusE_UiEFramebufferTargetE(uint v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate SpiceEngine.GLFWBindings.GLEnums.GraphicsResetStatus DEL_EGraphicsResetStatusE_();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate SpiceEngine.GLFWBindings.GLEnums.SyncStatus DEL_ESyncStatusE_StpSyncStpESyncObjectMaskEUl(SpiceEngine.GLFWBindings.GLStructs.Sync* v0, SpiceEngine.GLFWBindings.GLEnums.SyncObjectMask v1, ulong v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DEL_I_ERenderingModeE(SpiceEngine.GLFWBindings.GLEnums.RenderingMode v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DEL_I_UiCp(uint v0, char* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DEL_I_UiEProgramInterfaceECp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface v1, char* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DEL_I_UiEShaderTypeECp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ShaderType v1, char* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate SpiceEngine.GLFWBindings.GLStructs.Sync* DEL_StpSyncStp_ESyncConditionEESyncBehaviorFlagsE(SpiceEngine.GLFWBindings.GLEnums.SyncCondition v0, SpiceEngine.GLFWBindings.GLEnums.SyncBehaviorFlags v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint DEL_Ui_();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint DEL_Ui_EShaderTypeE(SpiceEngine.GLFWBindings.GLEnums.ShaderType v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint DEL_Ui_EShaderTypeEICpp(SpiceEngine.GLFWBindings.GLEnums.ShaderType v0, int v1, char** v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint DEL_Ui_I(int v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint DEL_Ui_UiCp(uint v0, char* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint DEL_Ui_UiEProgramInterfaceECp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface v1, char* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint DEL_Ui_UiEShaderTypeECp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ShaderType v1, char* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint DEL_Ui_UiIEpDebugSourceEpEpDebugTypeEpUipEpDebugSeverityEpIpCp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.DebugSource* v2, SpiceEngine.GLFWBindings.GLEnums.DebugType* v3, uint* v4, SpiceEngine.GLFWBindings.GLEnums.DebugSeverity* v5, int* v6, char* v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_B(bool v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_BBBB(bool v0, bool v1, bool v2, bool v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_Bp(bool* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_By(byte v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ByByBy(byte v0, byte v1, byte v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ByByByBy(byte v0, byte v1, byte v2, byte v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_Byp(byte* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_D(double v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_DD(double v0, double v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_DDD(double v0, double v1, double v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_DDDD(double v0, double v1, double v2, double v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_DDDDDD(double v0, double v1, double v2, double v3, double v4, double v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_Dp(double* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_DpDp(double* v0, double* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EAccumOpEF(SpiceEngine.GLFWBindings.GLEnums.AccumOp v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EAlphaFunctionEF(SpiceEngine.GLFWBindings.GLEnums.AlphaFunction v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EAttribMaskE(SpiceEngine.GLFWBindings.GLEnums.AttribMask v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBindTransformFeedbackTargetEUi(SpiceEngine.GLFWBindings.GLEnums.BindTransformFeedbackTarget v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBlendEquationModeEXTE(SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBlendEquationModeEXTEEBlendEquationModeEXTE(SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT v0, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBlendingFactorEEBlendingFactorE(SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v0, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBlendingFactorEEBlendingFactorEEBlendingFactorEEBlendingFactorE(SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v0, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v1, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v2, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferEIFI(SpiceEngine.GLFWBindings.GLEnums.Buffer v0, int v1, float v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferEIFp(SpiceEngine.GLFWBindings.GLEnums.Buffer v0, int v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferEIIp(SpiceEngine.GLFWBindings.GLEnums.Buffer v0, int v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferEIUip(SpiceEngine.GLFWBindings.GLEnums.Buffer v0, int v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferStorageTargetEEInternalFormatEEPixelFormatEEPixelTypeEVp(SpiceEngine.GLFWBindings.GLEnums.BufferStorageTarget v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v2, SpiceEngine.GLFWBindings.GLEnums.PixelType v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferStorageTargetEPVpEBufferStorageMaskE(SpiceEngine.GLFWBindings.GLEnums.BufferStorageTarget v0, IntPtr v1, void* v2, SpiceEngine.GLFWBindings.GLEnums.BufferStorageMask v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEEBufferPNameARBEIp(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEEBufferPNameARBELp(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB v1, long* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEEBufferPointerNameARBEVpp(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, SpiceEngine.GLFWBindings.GLEnums.BufferPointerNameARB v1, void** v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEEInternalFormatEPPEPixelFormatEEPixelTypeEVp(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, IntPtr v2, IntPtr v3, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v4, SpiceEngine.GLFWBindings.GLEnums.PixelType v5, void* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEPP(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, IntPtr v1, IntPtr v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEPPVp(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, IntPtr v1, IntPtr v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEPVpEBufferUsageARBE(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, IntPtr v1, void* v2, SpiceEngine.GLFWBindings.GLEnums.BufferUsageARB v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEUi(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEUiBp(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, uint v1, bool* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEUiIUip(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, uint v1, int v2, uint* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEUiIUipPpPp(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, uint v1, int v2, uint* v3, IntPtr* v4, IntPtr* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEUiUi(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, uint v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EBufferTargetARBEUiUiPP(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, uint v1, uint v2, IntPtr v3, IntPtr v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EClampColorTargetARBEEClampColorModeARBE(SpiceEngine.GLFWBindings.GLEnums.ClampColorTargetARB v0, SpiceEngine.GLFWBindings.GLEnums.ClampColorModeARB v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EClearBufferMaskE(SpiceEngine.GLFWBindings.GLEnums.ClearBufferMask v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EClientAttribMaskE(SpiceEngine.GLFWBindings.GLEnums.ClientAttribMask v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EClipControlOriginEEClipControlDepthE(SpiceEngine.GLFWBindings.GLEnums.ClipControlOrigin v0, SpiceEngine.GLFWBindings.GLEnums.ClipControlDepth v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EClipPlaneNameEDp(SpiceEngine.GLFWBindings.GLEnums.ClipPlaneName v0, double* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EColorPointerTypeEUi(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EColorPointerTypeEUip(SpiceEngine.GLFWBindings.GLEnums.ColorPointerType v0, uint* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EColorTableTargetEEPixelFormatEEPixelTypeEIVp(SpiceEngine.GLFWBindings.GLEnums.ColorTableTarget v0, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v1, SpiceEngine.GLFWBindings.GLEnums.PixelType v2, int v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EConvolutionTargetEEPixelFormatEEPixelTypeEIVp(SpiceEngine.GLFWBindings.GLEnums.ConvolutionTarget v0, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v1, SpiceEngine.GLFWBindings.GLEnums.PixelType v2, int v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ECopyBufferSubDataTargetEECopyBufferSubDataTargetEPPP(SpiceEngine.GLFWBindings.GLEnums.CopyBufferSubDataTarget v0, SpiceEngine.GLFWBindings.GLEnums.CopyBufferSubDataTarget v1, IntPtr v2, IntPtr v3, IntPtr v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ECullFaceModeE(SpiceEngine.GLFWBindings.GLEnums.CullFaceMode v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EDebugSourceEEDebugTypeEEDebugSeverityEIUipB(SpiceEngine.GLFWBindings.GLEnums.DebugSource v0, SpiceEngine.GLFWBindings.GLEnums.DebugType v1, SpiceEngine.GLFWBindings.GLEnums.DebugSeverity v2, int v3, uint* v4, bool v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EDebugSourceEEDebugTypeEUiEDebugSeverityEICp(SpiceEngine.GLFWBindings.GLEnums.DebugSource v0, SpiceEngine.GLFWBindings.GLEnums.DebugType v1, uint v2, SpiceEngine.GLFWBindings.GLEnums.DebugSeverity v3, int v4, char* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EDebugSourceEUiICp(SpiceEngine.GLFWBindings.GLEnums.DebugSource v0, uint v1, int v2, char* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EDepthFunctionE(SpiceEngine.GLFWBindings.GLEnums.DepthFunction v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EDrawBufferModeE(SpiceEngine.GLFWBindings.GLEnums.DrawBufferMode v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EEnableCapE(SpiceEngine.GLFWBindings.GLEnums.EnableCap v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EEnableCapEUi(SpiceEngine.GLFWBindings.GLEnums.EnableCap v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFogParameterEF(SpiceEngine.GLFWBindings.GLEnums.FogParameter v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFogParameterEFp(SpiceEngine.GLFWBindings.GLEnums.FogParameter v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFogParameterEI(SpiceEngine.GLFWBindings.GLEnums.FogParameter v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFogParameterEIp(SpiceEngine.GLFWBindings.GLEnums.FogParameter v0, int* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFogPointerTypeEXTEIVp(SpiceEngine.GLFWBindings.GLEnums.FogPointerTypeEXT v0, int v1, void* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEEFramebufferAttachmentEEFramebufferAttachmentParameterNameEIp(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEEFramebufferAttachmentEERenderbufferTargetEUi(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEEFramebufferAttachmentEETextureTargetEUiI(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, SpiceEngine.GLFWBindings.GLEnums.TextureTarget v2, uint v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEEFramebufferAttachmentEETextureTargetEUiII(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, SpiceEngine.GLFWBindings.GLEnums.TextureTarget v2, uint v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEEFramebufferAttachmentEUiI(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, uint v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEEFramebufferAttachmentEUiII(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, uint v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEEFramebufferAttachmentParameterNameEIp(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEEFramebufferParameterNameEI(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferParameterName v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEIEpInvalidateFramebufferAttachmentEp(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InvalidateFramebufferAttachment* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEIEpInvalidateFramebufferAttachmentEpIIII(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InvalidateFramebufferAttachment* v2, int v3, int v4, int v5, int v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFramebufferTargetEUi(SpiceEngine.GLFWBindings.GLEnums.FramebufferTarget v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EFrontFaceDirectionE(SpiceEngine.GLFWBindings.GLEnums.FrontFaceDirection v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetMultisamplePNameNVEUiFp(SpiceEngine.GLFWBindings.GLEnums.GetMultisamplePNameNV v0, uint v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPNameEBp(SpiceEngine.GLFWBindings.GLEnums.GetPName v0, bool* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPNameEDp(SpiceEngine.GLFWBindings.GLEnums.GetPName v0, double* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPNameEFp(SpiceEngine.GLFWBindings.GLEnums.GetPName v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPNameEIp(SpiceEngine.GLFWBindings.GLEnums.GetPName v0, int* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPNameELp(SpiceEngine.GLFWBindings.GLEnums.GetPName v0, long* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPNameEUiDp(SpiceEngine.GLFWBindings.GLEnums.GetPName v0, uint v1, double* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPNameEUiFp(SpiceEngine.GLFWBindings.GLEnums.GetPName v0, uint v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPNameEUiIp(SpiceEngine.GLFWBindings.GLEnums.GetPName v0, uint v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPNameEUiLp(SpiceEngine.GLFWBindings.GLEnums.GetPName v0, uint v1, long* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EGetPointervPNameEVpp(SpiceEngine.GLFWBindings.GLEnums.GetPointervPName v0, void** v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EHintTargetEEHintModeE(SpiceEngine.GLFWBindings.GLEnums.HintTarget v0, SpiceEngine.GLFWBindings.GLEnums.HintMode v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EHistogramTargetEBEPixelFormatEEPixelTypeEIVp(SpiceEngine.GLFWBindings.GLEnums.HistogramTarget v0, bool v1, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v2, SpiceEngine.GLFWBindings.GLEnums.PixelType v3, int v4, void* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EIndexPointerTypeEIVp(SpiceEngine.GLFWBindings.GLEnums.IndexPointerType v0, int v1, void* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EInterleavedArrayFormatEIVp(SpiceEngine.GLFWBindings.GLEnums.InterleavedArrayFormat v0, int v1, void* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ELightModelParameterEF(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ELightModelParameterEFp(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ELightModelParameterEI(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ELightModelParameterEIp(SpiceEngine.GLFWBindings.GLEnums.LightModelParameter v0, int* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ELightNameEELightParameterEF(SpiceEngine.GLFWBindings.GLEnums.LightName v0, SpiceEngine.GLFWBindings.GLEnums.LightParameter v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ELightNameEELightParameterEFp(SpiceEngine.GLFWBindings.GLEnums.LightName v0, SpiceEngine.GLFWBindings.GLEnums.LightParameter v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ELightNameEELightParameterEI(SpiceEngine.GLFWBindings.GLEnums.LightName v0, SpiceEngine.GLFWBindings.GLEnums.LightParameter v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ELightNameEELightParameterEIp(SpiceEngine.GLFWBindings.GLEnums.LightName v0, SpiceEngine.GLFWBindings.GLEnums.LightParameter v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ELogicOpE(SpiceEngine.GLFWBindings.GLEnums.LogicOp v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEDDIIDDIIDp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, double v1, double v2, int v3, int v4, double v5, double v6, int v7, int v8, double* v9);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEDDIIDp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, double v1, double v2, int v3, int v4, double* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEEGetMapQueryEDp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery v1, double* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEEGetMapQueryEFp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEEGetMapQueryEIp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, SpiceEngine.GLFWBindings.GLEnums.GetMapQuery v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEEMapQueryEIDp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, SpiceEngine.GLFWBindings.GLEnums.MapQuery v1, int v2, double* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEEMapQueryEIFp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, SpiceEngine.GLFWBindings.GLEnums.MapQuery v1, int v2, float* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEEMapQueryEIIp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, SpiceEngine.GLFWBindings.GLEnums.MapQuery v1, int v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEFFIIFFIIFp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, float v1, float v2, int v3, int v4, float v5, float v6, int v7, int v8, float* v9);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMapTargetEFFIIFp(SpiceEngine.GLFWBindings.GLEnums.MapTarget v0, float v1, float v2, int v3, int v4, float* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMaterialFaceEEColorMaterialParameterE(SpiceEngine.GLFWBindings.GLEnums.MaterialFace v0, SpiceEngine.GLFWBindings.GLEnums.ColorMaterialParameter v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMaterialFaceEEMaterialParameterEF(SpiceEngine.GLFWBindings.GLEnums.MaterialFace v0, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMaterialFaceEEMaterialParameterEFp(SpiceEngine.GLFWBindings.GLEnums.MaterialFace v0, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMaterialFaceEEMaterialParameterEI(SpiceEngine.GLFWBindings.GLEnums.MaterialFace v0, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMaterialFaceEEMaterialParameterEIp(SpiceEngine.GLFWBindings.GLEnums.MaterialFace v0, SpiceEngine.GLFWBindings.GLEnums.MaterialParameter v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMaterialFaceEEPolygonModeE(SpiceEngine.GLFWBindings.GLEnums.MaterialFace v0, SpiceEngine.GLFWBindings.GLEnums.PolygonMode v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMatrixModeE(SpiceEngine.GLFWBindings.GLEnums.MatrixMode v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMemoryBarrierMaskE(SpiceEngine.GLFWBindings.GLEnums.MemoryBarrierMask v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMeshMode1EII(SpiceEngine.GLFWBindings.GLEnums.MeshMode1 v0, int v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMeshMode2EIIII(SpiceEngine.GLFWBindings.GLEnums.MeshMode2 v0, int v1, int v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EMinmaxTargetEBEPixelFormatEEPixelTypeEIVp(SpiceEngine.GLFWBindings.GLEnums.MinmaxTarget v0, bool v1, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v2, SpiceEngine.GLFWBindings.GLEnums.PixelType v3, int v4, void* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ENormalPointerTypeEIVp(SpiceEngine.GLFWBindings.GLEnums.NormalPointerType v0, int v1, void* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ENormalPointerTypeEUi(SpiceEngine.GLFWBindings.GLEnums.NormalPointerType v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ENormalPointerTypeEUip(SpiceEngine.GLFWBindings.GLEnums.NormalPointerType v0, uint* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EObjectIdentifierEUiICp(SpiceEngine.GLFWBindings.GLEnums.ObjectIdentifier v0, uint v1, int v2, char* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EObjectIdentifierEUiIIpCp(SpiceEngine.GLFWBindings.GLEnums.ObjectIdentifier v0, uint v1, int v2, int* v3, char* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPatchParameterNameEFp(SpiceEngine.GLFWBindings.GLEnums.PatchParameterName v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPatchParameterNameEI(SpiceEngine.GLFWBindings.GLEnums.PatchParameterName v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelMapEFp(SpiceEngine.GLFWBindings.GLEnums.PixelMap v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelMapEIFp(SpiceEngine.GLFWBindings.GLEnums.PixelMap v0, int v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelMapEIUip(SpiceEngine.GLFWBindings.GLEnums.PixelMap v0, int v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelMapEIUsp(SpiceEngine.GLFWBindings.GLEnums.PixelMap v0, int v1, ushort* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelMapEUip(SpiceEngine.GLFWBindings.GLEnums.PixelMap v0, uint* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelMapEUsp(SpiceEngine.GLFWBindings.GLEnums.PixelMap v0, ushort* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelStoreParameterEF(SpiceEngine.GLFWBindings.GLEnums.PixelStoreParameter v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelStoreParameterEI(SpiceEngine.GLFWBindings.GLEnums.PixelStoreParameter v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelTransferParameterEF(SpiceEngine.GLFWBindings.GLEnums.PixelTransferParameter v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPixelTransferParameterEI(SpiceEngine.GLFWBindings.GLEnums.PixelTransferParameter v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPointParameterNameARBEF(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPointParameterNameARBEFp(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPointParameterNameARBEI(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPointParameterNameARBEIp(SpiceEngine.GLFWBindings.GLEnums.PointParameterNameARB v0, int* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeE(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEEDrawElementsTypeEVp(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v1, void* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEEDrawElementsTypeEVpII(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v1, void* v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEEDrawElementsTypeEVpPII(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v1, void* v2, IntPtr v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVp(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int v1, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpI(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int v1, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v2, void* v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpII(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int v1, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v2, void* v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIEDrawElementsTypeEVpIIUi(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int v1, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v2, void* v3, int v4, int v5, uint v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIEPrimitiveTypeEVpIUi(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int v1, SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v2, void* v3, int v4, uint v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEII(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIII(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int v1, int v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIIIUi(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int v1, int v2, int v3, uint v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIpEDrawElementsTypeEVppI(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int* v1, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v2, void** v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIpEDrawElementsTypeEVppIIp(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int* v1, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v2, void** v3, int v4, int* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEIpIpI(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, int* v1, int* v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEUi(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEUiI(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, uint v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEUiUi(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, uint v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEUiUiI(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, uint v1, uint v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEUiUiIEDrawElementsTypeEVp(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, uint v1, uint v2, int v3, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v4, void* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEUiUiIEDrawElementsTypeEVpI(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, uint v1, uint v2, int v3, SpiceEngine.GLFWBindings.GLEnums.DrawElementsType v4, void* v5, int v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEVp(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, void* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEVpII(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, void* v1, int v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EPrimitiveTypeEVpPII(SpiceEngine.GLFWBindings.GLEnums.PrimitiveType v0, void* v1, IntPtr v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EQueryTargetE(SpiceEngine.GLFWBindings.GLEnums.QueryTarget v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EQueryTargetEEQueryParameterNameEIp(SpiceEngine.GLFWBindings.GLEnums.QueryTarget v0, SpiceEngine.GLFWBindings.GLEnums.QueryParameterName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EQueryTargetEIUip(SpiceEngine.GLFWBindings.GLEnums.QueryTarget v0, int v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EQueryTargetEUi(SpiceEngine.GLFWBindings.GLEnums.QueryTarget v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EQueryTargetEUiEQueryParameterNameEIp(SpiceEngine.GLFWBindings.GLEnums.QueryTarget v0, uint v1, SpiceEngine.GLFWBindings.GLEnums.QueryParameterName v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EQueryTargetEUiUi(SpiceEngine.GLFWBindings.GLEnums.QueryTarget v0, uint v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EReadBufferModeE(SpiceEngine.GLFWBindings.GLEnums.ReadBufferMode v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ERenderbufferTargetEEInternalFormatEII(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, int v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ERenderbufferTargetEERenderbufferParameterNameEIp(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget v0, SpiceEngine.GLFWBindings.GLEnums.RenderbufferParameterName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ERenderbufferTargetEIEInternalFormatEII(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ERenderbufferTargetEUi(SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ESeparableTargetEEPixelFormatEEPixelTypeEIVpIVpVp(SpiceEngine.GLFWBindings.GLEnums.SeparableTarget v0, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v1, SpiceEngine.GLFWBindings.GLEnums.PixelType v2, int v3, void* v4, int v5, void* v6, void* v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EShaderTypeEEPrecisionTypeEIpIp(SpiceEngine.GLFWBindings.GLEnums.ShaderType v0, SpiceEngine.GLFWBindings.GLEnums.PrecisionType v1, int* v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EShaderTypeEIUip(SpiceEngine.GLFWBindings.GLEnums.ShaderType v0, int v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EShadingModelE(SpiceEngine.GLFWBindings.GLEnums.ShadingModel v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EStencilFaceDirectionEEStencilFunctionEIUi(SpiceEngine.GLFWBindings.GLEnums.StencilFaceDirection v0, SpiceEngine.GLFWBindings.GLEnums.StencilFunction v1, int v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EStencilFaceDirectionEEStencilOpEEStencilOpEEStencilOpE(SpiceEngine.GLFWBindings.GLEnums.StencilFaceDirection v0, SpiceEngine.GLFWBindings.GLEnums.StencilOp v1, SpiceEngine.GLFWBindings.GLEnums.StencilOp v2, SpiceEngine.GLFWBindings.GLEnums.StencilOp v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EStencilFaceDirectionEUi(SpiceEngine.GLFWBindings.GLEnums.StencilFaceDirection v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EStencilFunctionEIUi(SpiceEngine.GLFWBindings.GLEnums.StencilFunction v0, int v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EStencilOpEEStencilOpEEStencilOpE(SpiceEngine.GLFWBindings.GLEnums.StencilOp v0, SpiceEngine.GLFWBindings.GLEnums.StencilOp v1, SpiceEngine.GLFWBindings.GLEnums.StencilOp v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETexCoordPointerTypeEUi(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETexCoordPointerTypeEUip(SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType v0, uint* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureCoordNameEETextureGenParameterED(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName v0, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter v1, double v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureCoordNameEETextureGenParameterEDp(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName v0, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter v1, double* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureCoordNameEETextureGenParameterEF(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName v0, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureCoordNameEETextureGenParameterEFp(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName v0, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureCoordNameEETextureGenParameterEI(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName v0, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureCoordNameEETextureGenParameterEIp(SpiceEngine.GLFWBindings.GLEnums.TextureCoordName v0, SpiceEngine.GLFWBindings.GLEnums.TextureGenParameter v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureEnvTargetEETextureEnvParameterEF(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget v0, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureEnvTargetEETextureEnvParameterEFp(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget v0, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureEnvTargetEETextureEnvParameterEI(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget v0, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureEnvTargetEETextureEnvParameterEIp(SpiceEngine.GLFWBindings.GLEnums.TextureEnvTarget v0, SpiceEngine.GLFWBindings.GLEnums.TextureEnvParameter v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetE(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEEGetTextureParameterEFp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEEGetTextureParameterEIp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEEGetTextureParameterEUip(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEEInternalFormatEEInternalFormatPNameEIIp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormatPName v2, int v3, int* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEEInternalFormatEEInternalFormatPNameEILp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormatPName v2, int v3, long* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEEInternalFormatEUi(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEEInternalFormatEUiPP(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, uint v2, IntPtr v3, IntPtr v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEETextureParameterNameEF(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEETextureParameterNameEFp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEETextureParameterNameEI(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEETextureParameterNameEIp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEETextureParameterNameEUip(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEGetTextureParameterEFp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v2, float* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEGetTextureParameterEIp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEI(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEII(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIB(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, bool v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIEPixelFormatEEPixelTypeEVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v5, SpiceEngine.GLFWBindings.GLEnums.PixelType v6, void* v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIII(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIIB(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5, bool v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIIEPixelFormatEEPixelTypeEVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v6, SpiceEngine.GLFWBindings.GLEnums.PixelType v7, void* v8);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIII(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5, int v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIIIEPixelFormatEEPixelTypeEVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5, int v6, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v7, SpiceEngine.GLFWBindings.GLEnums.PixelType v8, void* v9);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIIII(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5, int v6, int v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIIIIVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5, int v6, int v7, void* v8);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIIIVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5, int v6, void* v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEInternalFormatEIIIVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5, void* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEPixelFormatEEPixelTypeEIVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v2, SpiceEngine.GLFWBindings.GLEnums.PixelType v3, int v4, void* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIEPixelFormatEEPixelTypeEVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v2, SpiceEngine.GLFWBindings.GLEnums.PixelType v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIIEPixelFormatEEPixelTypeEVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, int v3, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v4, SpiceEngine.GLFWBindings.GLEnums.PixelType v5, void* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIIEPixelFormatEIVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, int v3, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v4, int v5, void* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIIII(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, int v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIIIIEPixelFormatEEPixelTypeEVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, int v3, int v4, int v5, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v6, SpiceEngine.GLFWBindings.GLEnums.PixelType v7, void* v8);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIIIIEPixelFormatEIVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, int v3, int v4, int v5, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v6, int v7, void* v8);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIIIIII(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIIIIIIEPixelFormatEEPixelTypeEVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v8, SpiceEngine.GLFWBindings.GLEnums.PixelType v9, void* v10);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIIIIIIEPixelFormatEIVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v8, int v9, void* v10);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIIIIIII(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIIVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, int v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIUip(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEIVp(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, int v1, void* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureTargetEUi(SpiceEngine.GLFWBindings.GLEnums.TextureTarget v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitE(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitED(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, double v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEDD(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, double v1, double v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEDDD(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, double v1, double v2, double v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEDDDD(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, double v1, double v2, double v3, double v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEDp(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, double* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEETexCoordPointerTypeEUi(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEETexCoordPointerTypeEUip(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEF(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEFF(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, float v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEFFF(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, float v1, float v2, float v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEFFFF(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, float v1, float v2, float v3, float v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEFp(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEI(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEII(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, int v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEIII(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, int v1, int v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEIIII(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, int v1, int v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitEIp(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, int* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitES(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, short v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitESp(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, short* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitESS(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, short v1, short v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitESSS(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, short v1, short v2, short v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ETextureUnitESSSS(SpiceEngine.GLFWBindings.GLEnums.TextureUnit v0, short v1, short v2, short v3, short v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EVertexPointerTypeEUi(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EVertexPointerTypeEUip(SpiceEngine.GLFWBindings.GLEnums.VertexPointerType v0, uint* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_EVertexProvokingModeE(SpiceEngine.GLFWBindings.GLEnums.VertexProvokingMode v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_F(float v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_FB(float v0, bool v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_FF(float v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_FFF(float v0, float v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_FFFF(float v0, float v1, float v2, float v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_Fp(float* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_FpFp(float* v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_I(int v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IByp(int v0, byte* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_ID(int v0, double v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IDD(int v0, double v1, double v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IDDD(int v0, double v1, double v2, double v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IDDDD(int v0, double v1, double v2, double v3, double v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IDDIDD(int v0, double v1, double v2, int v3, double v4, double v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IEColorPointerTypeEIVp(int v0, SpiceEngine.GLFWBindings.GLEnums.ColorPointerType v1, int v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IEFeedbackTypeEFp(int v0, SpiceEngine.GLFWBindings.GLEnums.FeedbackType v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IEListNameTypeEVp(int v0, SpiceEngine.GLFWBindings.GLEnums.ListNameType v1, void* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IEpDrawBufferModeEp(int v0, SpiceEngine.GLFWBindings.GLEnums.DrawBufferMode* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IETexCoordPointerTypeEIVp(int v0, SpiceEngine.GLFWBindings.GLEnums.TexCoordPointerType v1, int v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IEVertexPointerTypeEIVp(int v0, SpiceEngine.GLFWBindings.GLEnums.VertexPointerType v1, int v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IF(int v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IFF(int v0, float v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IFFF(int v0, float v1, float v2, float v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IFFFF(int v0, float v1, float v2, float v3, float v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IFFIFF(int v0, float v1, float v2, int v3, float v4, float v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_II(int v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIBDp(int v0, int v1, bool v2, double* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIBFp(int v0, int v1, bool v2, float* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIDp(int v0, int v1, double* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIEPixelFormatEEPixelTypeEVp(int v0, int v1, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v2, SpiceEngine.GLFWBindings.GLEnums.PixelType v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIFFFFByp(int v0, int v1, float v2, float v3, float v4, float v5, byte* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIFp(int v0, int v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_III(int v0, int v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIII(int v0, int v1, int v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIIIEPixelCopyTypeE(int v0, int v1, int v2, int v3, SpiceEngine.GLFWBindings.GLEnums.PixelCopyType v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIIIEPixelFormatEEPixelTypeEIVp(int v0, int v1, int v2, int v3, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v4, SpiceEngine.GLFWBindings.GLEnums.PixelType v5, int v6, void* v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIIIEPixelFormatEEPixelTypeEVp(int v0, int v1, int v2, int v3, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v4, SpiceEngine.GLFWBindings.GLEnums.PixelType v5, void* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIIII(int v0, int v1, int v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIIIIIIIEClearBufferMaskEEBlitFramebufferFilterE(int v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, SpiceEngine.GLFWBindings.GLEnums.ClearBufferMask v8, SpiceEngine.GLFWBindings.GLEnums.BlitFramebufferFilter v9);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIIp(int v0, int v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IIUip(int v0, int v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_Ip(int* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IpIp(int* v0, int* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IUi(int v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IUip(int v0, uint* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IUipEShaderBinaryFormatEVpI(int v0, uint* v1, SpiceEngine.GLFWBindings.GLEnums.ShaderBinaryFormat v2, void* v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IUipFp(int v0, uint* v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IUiUi(int v0, uint v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IUiUiUi(int v0, uint v1, uint v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IUiUiUiUi(int v0, uint v1, uint v2, uint v3, uint v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IUs(int v0, ushort v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_IVp(int v0, void* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_P(IntPtr v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_S(short v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_Sp(short* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_SpSp(short* v0, short* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_SS(short v0, short v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_SSS(short v0, short v1, short v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_SSSS(short v0, short v1, short v2, short v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_StpSyncStp(SpiceEngine.GLFWBindings.GLStructs.Sync* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_StpSyncStpESyncBehaviorFlagsEUl(SpiceEngine.GLFWBindings.GLStructs.Sync* v0, SpiceEngine.GLFWBindings.GLEnums.SyncBehaviorFlags v1, ulong v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_StpSyncStpESyncParameterNameEIIpIp(SpiceEngine.GLFWBindings.GLStructs.Sync* v0, SpiceEngine.GLFWBindings.GLEnums.SyncParameterName v1, int v2, int* v3, int* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_Ui(uint v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiBBBB(uint v0, bool v1, bool v2, bool v3, bool v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiByByByBy(uint v0, byte v1, byte v2, byte v3, byte v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiByp(uint v0, byte* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiCpUiUipUip(uint v0, char* v1, uint v2, uint* v3, uint* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiD(uint v0, double v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiDD(uint v0, double v1, double v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiDDD(uint v0, double v1, double v2, double v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiDDDD(uint v0, double v1, double v2, double v3, double v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiDp(uint v0, double* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBlendEquationModeEXTE(uint v0, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBlendEquationModeEXTEEBlendEquationModeEXTE(uint v0, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT v1, SpiceEngine.GLFWBindings.GLEnums.BlendEquationModeEXT v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBlendingFactorEEBlendingFactorE(uint v0, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v1, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBlendingFactorEEBlendingFactorEEBlendingFactorEEBlendingFactorE(uint v0, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v1, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v2, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v3, SpiceEngine.GLFWBindings.GLEnums.BlendingFactor v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBufferEIFI(uint v0, SpiceEngine.GLFWBindings.GLEnums.Buffer v1, int v2, float v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBufferEIFp(uint v0, SpiceEngine.GLFWBindings.GLEnums.Buffer v1, int v2, float* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBufferEIIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.Buffer v1, int v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBufferEIUip(uint v0, SpiceEngine.GLFWBindings.GLEnums.Buffer v1, int v2, uint* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBufferPNameARBEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBufferPNameARBELp(uint v0, SpiceEngine.GLFWBindings.GLEnums.BufferPNameARB v1, long* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEBufferPointerNameARBEVpp(uint v0, SpiceEngine.GLFWBindings.GLEnums.BufferPointerNameARB v1, void** v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEColorBufferE(uint v0, SpiceEngine.GLFWBindings.GLEnums.ColorBuffer v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEConditionalRenderModeE(uint v0, SpiceEngine.GLFWBindings.GLEnums.ConditionalRenderMode v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiECopyImageSubDataTargetEIIIIUiECopyImageSubDataTargetEIIIIIII(uint v0, SpiceEngine.GLFWBindings.GLEnums.CopyImageSubDataTarget v1, int v2, int v3, int v4, int v5, uint v6, SpiceEngine.GLFWBindings.GLEnums.CopyImageSubDataTarget v7, int v8, int v9, int v10, int v11, int v12, int v13, int v14);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEFramebufferAttachmentEEFramebufferAttachmentParameterNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachmentParameterName v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEFramebufferAttachmentEERenderbufferTargetEUi(uint v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, SpiceEngine.GLFWBindings.GLEnums.RenderbufferTarget v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEFramebufferAttachmentEUiI(uint v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, uint v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEFramebufferAttachmentEUiII(uint v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment v1, uint v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEFramebufferParameterNameEI(uint v0, SpiceEngine.GLFWBindings.GLEnums.FramebufferParameterName v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEGetFramebufferParameterEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.GetFramebufferParameter v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEGetTextureParameterEFp(uint v0, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEGetTextureParameterEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEGetTextureParameterEUip(uint v0, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEInternalFormatEEPixelFormatEEPixelTypeEVp(uint v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v2, SpiceEngine.GLFWBindings.GLEnums.PixelType v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEInternalFormatEII(uint v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, int v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEInternalFormatEPPEPixelFormatEEPixelTypeEVp(uint v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, IntPtr v2, IntPtr v3, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v4, SpiceEngine.GLFWBindings.GLEnums.PixelType v5, void* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEInternalFormatEUi(uint v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEInternalFormatEUiPP(uint v0, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v1, uint v2, IntPtr v3, IntPtr v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEListModeE(uint v0, SpiceEngine.GLFWBindings.GLEnums.ListMode v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEPipelineParameterNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.PipelineParameterName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEProgramInterfaceEEProgramInterfacePNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface v1, SpiceEngine.GLFWBindings.GLEnums.ProgramInterfacePName v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEProgramInterfaceEUiIEpProgramResourcePropertyEpIIpIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface v1, uint v2, int v3, SpiceEngine.GLFWBindings.GLEnums.ProgramResourceProperty* v4, int v5, int* v6, int* v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEProgramInterfaceEUiIIpCp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ProgramInterface v1, uint v2, int v3, int* v4, char* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEProgramParameterPNameEI(uint v0, SpiceEngine.GLFWBindings.GLEnums.ProgramParameterPName v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEProgramPropertyARBEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ProgramPropertyARB v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEQueryCounterTargetE(uint v0, SpiceEngine.GLFWBindings.GLEnums.QueryCounterTarget v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEQueryObjectParameterNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEQueryObjectParameterNameELp(uint v0, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName v1, long* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEQueryObjectParameterNameEUip(uint v0, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEQueryObjectParameterNameEUlp(uint v0, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName v1, ulong* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiERenderbufferParameterNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.RenderbufferParameterName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiESamplerParameterFEF(uint v0, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterF v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiESamplerParameterFEFp(uint v0, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterF v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiESamplerParameterIEI(uint v0, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiESamplerParameterIEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiESamplerParameterIEUip(uint v0, SpiceEngine.GLFWBindings.GLEnums.SamplerParameterI v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEShaderParameterNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ShaderParameterName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEShaderTypeEEProgramStagePNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ShaderType v1, SpiceEngine.GLFWBindings.GLEnums.ProgramStagePName v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEShaderTypeEUiESubroutineParameterNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ShaderType v1, uint v2, SpiceEngine.GLFWBindings.GLEnums.SubroutineParameterName v3, int* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEShaderTypeEUiIIpCp(uint v0, SpiceEngine.GLFWBindings.GLEnums.ShaderType v1, uint v2, int v3, int* v4, char* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiETextureParameterNameEF(uint v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiETextureParameterNameEFp(uint v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiETextureParameterNameEI(uint v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiETextureParameterNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiETextureParameterNameEUip(uint v0, SpiceEngine.GLFWBindings.GLEnums.TextureParameterName v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiETextureTargetEUiEInternalFormatEUiUiUiUi(uint v0, SpiceEngine.GLFWBindings.GLEnums.TextureTarget v1, uint v2, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v3, uint v4, uint v5, uint v6, uint v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiETransformFeedbackPNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiETransformFeedbackPNameEUiIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName v1, uint v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiETransformFeedbackPNameEUiLp(uint v0, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackPName v1, uint v2, long* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEUseProgramStageMaskEUi(uint v0, SpiceEngine.GLFWBindings.GLEnums.UseProgramStageMask v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexArrayPNameEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexAttribEnumEDp(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum v1, double* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexAttribEnumEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexAttribEnumEUip(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexAttribEnum v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexAttribPointerPropertyARBEVpp(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerPropertyARB v1, void** v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexAttribPointerTypeEBUi(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType v1, bool v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexAttribPointerTypeEBUip(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType v1, bool v2, uint* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexAttribPropertyARBEDp(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB v1, double* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexAttribPropertyARBEFp(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiEVertexAttribPropertyARBEIp(uint v0, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPropertyARB v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiF(uint v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiFF(uint v0, float v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiFFF(uint v0, float v1, float v2, float v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiFFFF(uint v0, float v1, float v2, float v3, float v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiFp(uint v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiI(uint v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiICppETransformFeedbackBufferModeE(uint v0, int v1, char** v2, SpiceEngine.GLFWBindings.GLEnums.TransformFeedbackBufferMode v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiICppIp(uint v0, int v1, char** v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiICppUip(uint v0, int v1, char** v2, uint* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiID(uint v0, int v1, double v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIDD(uint v0, int v1, double v2, double v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIDDD(uint v0, int v1, double v2, double v3, double v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIDDDD(uint v0, int v1, double v2, double v3, double v4, double v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIDp(uint v0, int v1, double* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEGetTextureParameterEFp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v2, float* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEGetTextureParameterEIp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.GetTextureParameter v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEInternalFormatEI(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEInternalFormatEII(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEInternalFormatEIIB(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, bool v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEInternalFormatEIII(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEInternalFormatEIIIB(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v2, int v3, int v4, int v5, bool v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEpColorBufferEp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.ColorBuffer* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEpFramebufferAttachmentEp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEpFramebufferAttachmentEpIIII(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.FramebufferAttachment* v2, int v3, int v4, int v5, int v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEPixelFormatEEPixelTypeEIVp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v2, SpiceEngine.GLFWBindings.GLEnums.PixelType v3, int v4, void* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEPixelFormatEEPixelTypeEVp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v2, SpiceEngine.GLFWBindings.GLEnums.PixelType v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEVertexAttribITypeEIVp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.VertexAttribIType v2, int v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEVertexAttribITypeEUi(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.VertexAttribIType v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEVertexAttribLTypeEIVp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.VertexAttribLType v2, int v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEVertexAttribLTypeEUi(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.VertexAttribLType v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEVertexAttribPointerTypeEBIVp(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.VertexAttribPointerType v2, bool v3, int v4, void* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIEVertexAttribTypeEBUi(uint v0, int v1, SpiceEngine.GLFWBindings.GLEnums.VertexAttribType v2, bool v3, uint v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIF(uint v0, int v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIFF(uint v0, int v1, float v2, float v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIFFF(uint v0, int v1, float v2, float v3, float v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIFFFF(uint v0, int v1, float v2, float v3, float v4, float v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIFp(uint v0, int v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiII(uint v0, int v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIBDp(uint v0, int v1, int v2, bool v3, double* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIBFp(uint v0, int v1, int v2, bool v3, float* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIDp(uint v0, int v1, int v2, double* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIFp(uint v0, int v1, int v2, float* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIII(uint v0, int v1, int v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIEPixelFormatEEPixelTypeEVp(uint v0, int v1, int v2, int v3, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v4, SpiceEngine.GLFWBindings.GLEnums.PixelType v5, void* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIEPixelFormatEIVp(uint v0, int v1, int v2, int v3, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v4, int v5, void* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIII(uint v0, int v1, int v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIII(uint v0, int v1, int v2, int v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIIIEPixelFormatEEPixelTypeEVp(uint v0, int v1, int v2, int v3, int v4, int v5, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v6, SpiceEngine.GLFWBindings.GLEnums.PixelType v7, void* v8);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIIIEPixelFormatEIVp(uint v0, int v1, int v2, int v3, int v4, int v5, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v6, int v7, void* v8);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIIIII(uint v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIIIIIEPixelFormatEEPixelTypeEIVp(uint v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v8, SpiceEngine.GLFWBindings.GLEnums.PixelType v9, int v10, void* v11);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIIIIIEPixelFormatEEPixelTypeEVp(uint v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v8, SpiceEngine.GLFWBindings.GLEnums.PixelType v9, void* v10);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIIIIIEPixelFormatEIVp(uint v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, SpiceEngine.GLFWBindings.GLEnums.PixelFormat v8, int v9, void* v10);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIIIIII(uint v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIIIIIIVp(uint v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, void* v9);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIIp(uint v0, int v1, int v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIp(uint v0, int v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIpCp(uint v0, int v1, int* v2, char* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIpIpVp(uint v0, int v1, int* v2, int* v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIpUip(uint v0, int v1, int* v2, uint* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIUip(uint v0, int v1, int v2, uint* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIIVp(uint v0, int v1, int v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIp(uint v0, int* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIUi(uint v0, int v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIUip(uint v0, int v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIUipEUniformPNameEIp(uint v0, int v1, uint* v2, SpiceEngine.GLFWBindings.GLEnums.UniformPName v3, int* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIUipPpIp(uint v0, int v1, uint* v2, IntPtr* v3, int* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIUiUi(uint v0, int v1, uint v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIUiUiUi(uint v0, int v1, uint v2, uint v3, uint v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIUiUiUiUi(uint v0, int v1, uint v2, uint v3, uint v4, uint v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiIVpI(uint v0, int v1, void* v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_Uip(uint* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiPP(uint v0, IntPtr v1, IntPtr v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiPPVp(uint v0, IntPtr v1, IntPtr v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiPVpEBufferStorageMaskE(uint v0, IntPtr v1, void* v2, SpiceEngine.GLFWBindings.GLEnums.BufferStorageMask v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiPVpEVertexBufferObjectUsageE(uint v0, IntPtr v1, void* v2, SpiceEngine.GLFWBindings.GLEnums.VertexBufferObjectUsage v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiS(uint v0, short v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiSp(uint v0, short* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiSS(uint v0, short v1, short v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiSSS(uint v0, short v1, short v2, short v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiSSSS(uint v0, short v1, short v2, short v3, short v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUi(uint v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiCp(uint v0, uint v1, char* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiEAtomicCounterBufferPNameEIp(uint v0, uint v1, SpiceEngine.GLFWBindings.GLEnums.AtomicCounterBufferPName v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiEQueryObjectParameterNameEP(uint v0, uint v1, SpiceEngine.GLFWBindings.GLEnums.QueryObjectParameterName v2, IntPtr v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiEUniformBlockPNameEIp(uint v0, uint v1, SpiceEngine.GLFWBindings.GLEnums.UniformBlockPName v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiEVertexArrayPNameEIp(uint v0, uint v1, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiEVertexArrayPNameELp(uint v0, uint v1, SpiceEngine.GLFWBindings.GLEnums.VertexArrayPName v2, long* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiIBIEBufferAccessARBEEInternalFormatE(uint v0, uint v1, int v2, bool v3, int v4, SpiceEngine.GLFWBindings.GLEnums.BufferAccessARB v5, SpiceEngine.GLFWBindings.GLEnums.InternalFormat v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiIEVertexAttribITypeEUi(uint v0, uint v1, int v2, SpiceEngine.GLFWBindings.GLEnums.VertexAttribIType v3, uint v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiIEVertexAttribLTypeEUi(uint v0, uint v1, int v2, SpiceEngine.GLFWBindings.GLEnums.VertexAttribLType v3, uint v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiIEVertexAttribTypeEBUi(uint v0, uint v1, int v2, SpiceEngine.GLFWBindings.GLEnums.VertexAttribType v3, bool v4, uint v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiIIIIIIIIEClearBufferMaskEEBlitFramebufferFilterE(uint v0, uint v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, int v9, SpiceEngine.GLFWBindings.GLEnums.ClearBufferMask v10, SpiceEngine.GLFWBindings.GLEnums.BlitFramebufferFilter v11);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiIIpCp(uint v0, uint v1, int v2, int* v3, char* v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiIIpIpEpAttributeTypeEpCp(uint v0, uint v1, int v2, int* v3, int* v4, SpiceEngine.GLFWBindings.GLEnums.AttributeType* v5, char* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiIIpIpEpUniformTypeEpCp(uint v0, uint v1, int v2, int* v3, int* v4, SpiceEngine.GLFWBindings.GLEnums.UniformType* v5, char* v6);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiIUipPpIp(uint v0, uint v1, int v2, uint* v3, IntPtr* v4, int* v5);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUip(uint v0, uint* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiPI(uint v0, uint v1, IntPtr v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiPPP(uint v0, uint v1, IntPtr v2, IntPtr v3, IntPtr v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiUi(uint v0, uint v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiUiCp(uint v0, uint v1, uint v2, char* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiUiPI(uint v0, uint v1, uint v2, IntPtr v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiUiPP(uint v0, uint v1, uint v2, IntPtr v3, IntPtr v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiUiUi(uint v0, uint v1, uint v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUiUiUiUi(uint v0, uint v1, uint v2, uint v3, uint v4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UiUsp(uint v0, ushort* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_Usp(ushort* v0);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UsUsUs(ushort v0, ushort v1, ushort v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_UsUsUsUs(ushort v0, ushort v1, ushort v2, ushort v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_VpICp(void* v0, int v1, char* v2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_VpIIpCp(void* v0, int v1, int* v2, char* v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void DEL_V_VpVp(void* v0, void* v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void* DEL_Vp_EBufferTargetARBEEBufferAccessARBE(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, SpiceEngine.GLFWBindings.GLEnums.BufferAccessARB v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void* DEL_Vp_EBufferTargetARBEPPEMapBufferAccessMaskE(SpiceEngine.GLFWBindings.GLEnums.BufferTargetARB v0, IntPtr v1, IntPtr v2, SpiceEngine.GLFWBindings.GLEnums.MapBufferAccessMask v3);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void* DEL_Vp_UiEBufferAccessARBE(uint v0, SpiceEngine.GLFWBindings.GLEnums.BufferAccessARB v1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void* DEL_Vp_UiPPEMapBufferAccessMaskE(uint v0, IntPtr v1, IntPtr v2, SpiceEngine.GLFWBindings.GLEnums.MapBufferAccessMask v3);
    }
}
