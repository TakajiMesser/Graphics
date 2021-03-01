using System;
using System.Runtime.InteropServices;

namespace SpiceEngine.GLFWBindings
{
    public static unsafe class OldGL
    {
        public const int GL_FRAMEBUFFER = 0x8D40;
        public const int GL_RENDERBUFFER = 0x8D41;
        public const int GL_CLAMP_READ_COLOR = 0x891C;

        private static DEL_V_I _glActiveTexture;
        private static DEL_V_UiUi _glAttachShader;
        private static DEL_V_UiI _glBeginConditionalRender;
        private static DEL_V_IUi _glBeginQuery;
        private static DEL_V_I _glBeginTransformFeedback;
        private static DEL_V_UiUiByp _glBindAttribLocation;
        private static DEL_V_IUi _glBindBuffer;
        private static DEL_V_IUiUi _glBindBufferBase;
        private static DEL_V_IUiUiPP _glBindBufferRange;
        private static DEL_V_UiUiByp _glBindFragDataLocation;
        private static DEL_V_UiUiUiByp _glBindFragDataLocationIndexed;
        private static DEL_V_IUi _glBindFramebuffer;
        private static DEL_V_IUi _glBindRenderbuffer;
        private static DEL_V_UiUi _glBindSampler;
        private static DEL_V_IUi _glBindTexture;
        private static DEL_V_Ui _glBindVertexArray;
        private static DEL_V_FFFF _glBlendColor;
        private static DEL_V_I _glBlendEquation;
        private static DEL_V_II _glBlendEquationSeparate;
        private static DEL_V_II _glBlendFunc;
        private static DEL_V_IIII _glBlendFuncSeparate;
        private static DEL_V_IIIIIIIIUiI _glBlitFramebuffer;
        private static DEL_V_IPVpI _glBufferData;
        private static DEL_V_IPPVp _glBufferSubData;
        private static DEL_I_I _glCheckFramebufferStatus;
        private static DEL_V_II _glClampColor;
        private static DEL_V_Ui _glClear;
        private static DEL_V_IIFI _glClearBufferfi;
        private static DEL_V_IIFp _glClearBufferfv;
        private static DEL_V_IIIp _glClearBufferiv;
        private static DEL_V_IIUip _glClearBufferuiv;
        private static DEL_V_FFFF _glClearColor;
        private static DEL_V_D _glClearDepth;
        private static DEL_V_I _glClearStencil;
        private static DEL_I_PUiUl _glClientWaitSync;
        private static DEL_V_BBBB _glColorMask;
        private static DEL_V_UiBBBB _glColorMaski;
        private static DEL_V_IUi _glColorP3ui;
        private static DEL_V_IUip _glColorP3uiv;
        private static DEL_V_IUi _glColorP4ui;
        private static DEL_V_IUip _glColorP4uiv;
        private static DEL_V_Ui _glCompileShader;
        private static DEL_V_IIIIIIVp _glCompressedTexImage1D;
        private static DEL_V_IIIIIIIVp _glCompressedTexImage2D;
        private static DEL_V_IIIIIIIIVp _glCompressedTexImage3D;
        private static DEL_V_IIIIIIVp _glCompressedTexSubImage1D;
        private static DEL_V_IIIIIIIIVp _glCompressedTexSubImage2D;
        private static DEL_V_IIIIIIIIIIVp _glCompressedTexSubImage3D;
        private static DEL_V_IIPPP _glCopyBufferSubData;
        private static DEL_V_IIIIIII _glCopyTexImage1D;
        private static DEL_V_IIIIIIII _glCopyTexImage2D;
        private static DEL_V_IIIIII _glCopyTexSubImage1D;
        private static DEL_V_IIIIIIII _glCopyTexSubImage2D;
        private static DEL_V_IIIIIIIII _glCopyTexSubImage3D;
        private static DEL_Ui_ _glCreateProgram;
        private static DEL_Ui_I _glCreateShader;
        private static DEL_V_I _glCullFace;
        private static DEL_V_IUip _glDeleteBuffers;
        private static DEL_V_IUip _glDeleteFramebuffers;
        private static DEL_V_Ui _glDeleteProgram;
        private static DEL_V_IUip _glDeleteQueries;
        private static DEL_V_IUip _glDeleteRenderbuffers;
        private static DEL_V_IUip _glDeleteSamplers;
        private static DEL_V_Ui _glDeleteShader;
        private static DEL_V_P _glDeleteSync;
        private static DEL_V_IUip _glDeleteTextures;
        private static DEL_V_IUip _glDeleteVertexArrays;
        private static DEL_V_I _glDepthFunc;
        private static DEL_V_B _glDepthMask;
        private static DEL_V_DD _glDepthRange;
        private static DEL_V_UiUi _glDetachShader;
        private static DEL_V_I _glDisable;
        private static DEL_V_IUi _glDisablei;
        private static DEL_V_Ui _glDisableVertexAttribArray;
        private static DEL_V_III _glDrawArrays;
        private static DEL_V_IIII _glDrawArraysInstanced;
        private static DEL_V_I _glDrawBuffer;
        private static DEL_V_IIp _glDrawBuffers;
        private static DEL_V_IIIVp _glDrawElements;
        private static DEL_V_IIIVpI _glDrawElementsBaseVertex;
        private static DEL_V_IIIVpI _glDrawElementsInstanced;
        private static DEL_V_IIIVpII _glDrawElementsInstancedBaseVertex;
        private static DEL_V_IUiUiIIVp _glDrawRangeElements;
        private static DEL_V_IUiUiIIVpI _glDrawRangeElementsBaseVertex;
        private static DEL_V_I _glEnable;
        private static DEL_V_IUi _glEnablei;
        private static DEL_V_Ui _glEnableVertexAttribArray;
        private static DEL_V_ _glEndConditionalRender;
        private static DEL_V_I _glEndQuery;
        private static DEL_V_ _glEndTransformFeedback;
        private static DEL_P_IUi _glFenceSync;
        private static DEL_V_ _glFinish;
        private static DEL_V_ _glFlush;
        private static DEL_V_IPP _glFlushMappedBufferRange;
        private static DEL_V_IIIUi _glFramebufferRenderbuffer;
        private static DEL_V_IIUiI _glFramebufferTexture;
        private static DEL_V_IIIUiI _glFramebufferTexture1D;
        private static DEL_V_IIIUiI _glFramebufferTexture2D;
        private static DEL_V_IIIUiII _glFramebufferTexture3D;
        private static DEL_V_IIUiII _glFramebufferTextureLayer;
        private static DEL_V_I _glFrontFace;
        private static DEL_V_IUip _glGenBuffers;
        private static DEL_V_I _glGenerateMipmap;
        private static DEL_V_IUip _glGenFramebuffers;
        private static DEL_V_IUip _glGenQueries;
        private static DEL_V_IUip _glGenRenderbuffers;
        private static DEL_V_IUip _glGenSamplers;
        private static DEL_V_IUip _glGenTextures;
        private static DEL_V_IUip _glGenVertexArrays;
        private static DEL_V_UiUiIOiOiOiP _glGetActiveAttrib;
        private static DEL_V_UiUiIOiOiOiP _glGetActiveUniform;
        private static DEL_V_UiUiIIp _glGetActiveUniformBlockiv;
        private static DEL_V_UiUiIIpByp _glGetActiveUniformBlockName;
        private static DEL_V_UiUiIIpByp _glGetActiveUniformName;
        private static DEL_V_UiIUipIIp _glGetActiveUniformsiv;
        private static DEL_V_UiIIpUip _glGetAttachedShaders;
        private static DEL_I_UiByp _glGetAttribLocation;
        private static DEL_V_IUiBp _glGetBooleani_v;
        private static DEL_V_IBp _glGetBooleanv;
        private static DEL_V_IILp _glGetBufferParameteri64v;
        private static DEL_V_IIIp _glGetBufferParameteriv;
        private static DEL_V_IIOp _glGetBufferPointerv;
        private static DEL_V_IPPVp _glGetBufferSubData;
        private static DEL_V_IIVp _glGetCompressedTexImage;
        private static DEL_V_IDp _glGetDoublev;
        private static DEL_I_ _glGetError;
        private static DEL_V_IFp _glGetFloatv;
        private static DEL_I_UiByp _glGetFragDataIndex;
        private static DEL_I_UiByp _glGetFragDataLocation;
        private static DEL_V_IIIIp _glGetFramebufferAttachmentParameteriv;
        private static DEL_V_IUiLp _glGetInteger64i_v;
        private static DEL_V_ILp _glGetInteger64v;
        private static DEL_V_IUiIp _glGetIntegeri_v;
        private static DEL_V_IIp _glGetIntegerv;
        private static DEL_V_IUiFp _glGetMultisamplefv;
        private static DEL_V_UiIIpByp _glGetProgramInfoLog;
        private static DEL_V_UiIIp _glGetProgramiv;
        private static DEL_V_IIIp _glGetQueryiv;
        private static DEL_V_UiILp _glGetQueryObjecti64v;
        private static DEL_V_UiIIp _glGetQueryObjectiv;
        private static DEL_V_UiIUlp _glGetQueryObjectui64v;
        private static DEL_V_UiIUip _glGetQueryObjectuiv;
        private static DEL_V_IIIp _glGetRenderbufferParameteriv;
        private static DEL_V_UiIFp _glGetSamplerParameterfv;
        private static DEL_V_UiIIp _glGetSamplerParameterIiv;
        private static DEL_V_UiIUip _glGetSamplerParameterIuiv;
        private static DEL_V_UiIIp _glGetSamplerParameteriv;
        private static DEL_V_UiIIpByp _glGetShaderInfoLog;
        private static DEL_V_UiIIp _glGetShaderiv;
        private static DEL_V_UiIIpByp _glGetShaderSource;
        private static DEL_Byp_I _glGetString;
        private static DEL_Byp_IUi _glGetStringi;
        private static DEL_V_PIIIpIp _glGetSynciv;
        private static DEL_V_IIIIVp _glGetTexImage;
        private static DEL_V_IIIFp _glGetTexLevelParameterfv;
        private static DEL_V_IIIIp _glGetTexLevelParameteriv;
        private static DEL_V_IIFp _glGetTexParameterfv;
        private static DEL_V_IIIp _glGetTexParameterIiv;
        private static DEL_V_IIUip _glGetTexParameterIuiv;
        private static DEL_V_IIIp _glGetTexParameteriv;
        private static DEL_V_UiUiIOiOiOiP _glGetTransformFeedbackVarying;
        private static DEL_Ui_UiByp _glGetUniformBlockIndex;
        private static DEL_V_UiIFp _glGetUniformfv;
        private static DEL_V_UiIByppUip _glGetUniformIndices;
        private static DEL_V_UiIIp _glGetUniformiv;
        private static DEL_I_UiByp _glGetUniformLocation;
        private static DEL_V_UiIUip _glGetUniformuiv;
        private static DEL_V_UiIDp _glGetVertexAttribdv;
        private static DEL_V_UiIFp _glGetVertexAttribfv;
        private static DEL_V_UiIIp _glGetVertexAttribIiv;
        private static DEL_V_UiIUip _glGetVertexAttribIuiv;
        private static DEL_V_UiIIp _glGetVertexAttribiv;
        private static DEL_V_UiIOp _glGetVertexAttribPointerv;
        private static DEL_V_II _glHint;
        private static DEL_B_Ui _glIsBuffer;
        private static DEL_B_I _glIsEnabled;
        private static DEL_B_IUi _glIsEnabledi;
        private static DEL_B_Ui _glIsFramebuffer;
        private static DEL_B_Ui _glIsProgram;
        private static DEL_B_Ui _glIsQuery;
        private static DEL_B_Ui _glIsRenderbuffer;
        private static DEL_B_Ui _glIsSampler;
        private static DEL_B_Ui _glIsShader;
        private static DEL_B_P _glIsSync;
        private static DEL_B_Ui _glIsTexture;
        private static DEL_B_Ui _glIsVertexArray;
        private static DEL_V_F _glLineWidth;
        private static DEL_V_Ui _glLinkProgram;
        private static DEL_V_I _glLogicOp;
        private static DEL_Vp_II _glMapBuffer;
        private static DEL_Vp_IPPUi _glMapBufferRange;
        private static DEL_V_IIpIpI _glMultiDrawArrays;
        private static DEL_V_IIpIVppI _glMultiDrawElements;
        private static DEL_V_IIpIVppIIp _glMultiDrawElementsBaseVertex;
        private static DEL_V_IIUi _glMultiTexCoordP1ui;
        private static DEL_V_IIUip _glMultiTexCoordP1uiv;
        private static DEL_V_IIUi _glMultiTexCoordP2ui;
        private static DEL_V_IIUip _glMultiTexCoordP2uiv;
        private static DEL_V_IIUi _glMultiTexCoordP3ui;
        private static DEL_V_IIUip _glMultiTexCoordP3uiv;
        private static DEL_V_IIUi _glMultiTexCoordP4ui;
        private static DEL_V_IIUip _glMultiTexCoordP4uiv;
        private static DEL_V_IUi _glNormalP3ui;
        private static DEL_V_IUip _glNormalP3uiv;
        private static DEL_V_IF _glPixelStoref;
        private static DEL_V_II _glPixelStorei;
        private static DEL_V_IF _glPointParameterf;
        private static DEL_V_IFp _glPointParameterfv;
        private static DEL_V_II _glPointParameteri;
        private static DEL_V_IIp _glPointParameteriv;
        private static DEL_V_F _glPointSize;
        private static DEL_V_II _glPolygonMode;
        private static DEL_V_FF _glPolygonOffset;
        private static DEL_V_Ui _glPrimitiveRestartIndex;
        private static DEL_V_I _glProvokingVertex;
        private static DEL_V_UiI _glQueryCounter;
        private static DEL_V_I _glReadBuffer;
        private static DEL_V_IIIIIIVp _glReadPixels;
        private static DEL_V_IIII _glRenderbufferStorage;
        private static DEL_V_IIIII _glRenderbufferStorageMultisample;
        private static DEL_V_FB _glSampleCoverage;
        private static DEL_V_UiUi _glSampleMaski;
        private static DEL_V_UiIF _glSamplerParameterf;
        private static DEL_V_UiIFp _glSamplerParameterfv;
        private static DEL_V_UiII _glSamplerParameteri;
        private static DEL_V_UiIIp _glSamplerParameterIiv;
        private static DEL_V_UiIUip _glSamplerParameterIuiv;
        private static DEL_V_UiIIp _glSamplerParameteriv;
        private static DEL_V_IIII _glScissor;
        private static DEL_V_IUi _glSecondaryColorP3ui;
        private static DEL_V_IUip _glSecondaryColorP3uiv;
        private static DEL_V_UiIByppIp _glShaderSource;
        private static DEL_V_IIUi _glStencilFunc;
        private static DEL_V_IIIUi _glStencilFuncSeparate;
        private static DEL_V_Ui _glStencilMask;
        private static DEL_V_IUi _glStencilMaskSeparate;
        private static DEL_V_III _glStencilOp;
        private static DEL_V_IIII _glStencilOpSeparate;
        private static DEL_V_IIUi _glTexBuffer;
        private static DEL_V_IUi _glTexCoordP1ui;
        private static DEL_V_IUip _glTexCoordP1uiv;
        private static DEL_V_IUi _glTexCoordP2ui;
        private static DEL_V_IUip _glTexCoordP2uiv;
        private static DEL_V_IUi _glTexCoordP3ui;
        private static DEL_V_IUip _glTexCoordP3uiv;
        private static DEL_V_IUi _glTexCoordP4ui;
        private static DEL_V_IUip _glTexCoordP4uiv;
        private static DEL_V_IIIIIIIVp _glTexImage1D;
        private static DEL_V_IIIIIIIIVp _glTexImage2D;
        private static DEL_V_IIIIIB _glTexImage2DMultisample;
        private static DEL_V_IIIIIIIIIVp _glTexImage3D;
        private static DEL_V_IIIIIIB _glTexImage3DMultisample;
        private static DEL_V_IIF _glTexParameterf;
        private static DEL_V_IIFp _glTexParameterfv;
        private static DEL_V_III _glTexParameteri;
        private static DEL_V_IIIp _glTexParameterIiv;
        private static DEL_V_IIUip _glTexParameterIuiv;
        private static DEL_V_IIIp _glTexParameteriv;
        private static DEL_V_IIIIIIVp _glTexSubImage1D;
        private static DEL_V_IIIIIIIIVp _glTexSubImage2D;
        private static DEL_V_IIIIIIIIIIVp _glTexSubImage3D;
        private static DEL_V_UiIByppI _glTransformFeedbackVaryings;
        private static DEL_V_IF _glUniform1f;
        private static DEL_V_IIFp _glUniform1fv;
        private static DEL_V_II _glUniform1i;
        private static DEL_V_IIIp _glUniform1iv;
        private static DEL_V_IUi _glUniform1ui;
        private static DEL_V_IIUip _glUniform1uiv;
        private static DEL_V_IFF _glUniform2f;
        private static DEL_V_IIFp _glUniform2fv;
        private static DEL_V_III _glUniform2i;
        private static DEL_V_IIIp _glUniform2iv;
        private static DEL_V_IUiUi _glUniform2ui;
        private static DEL_V_IIUip _glUniform2uiv;
        private static DEL_V_IFFF _glUniform3f;
        private static DEL_V_IIFp _glUniform3fv;
        private static DEL_V_IIII _glUniform3i;
        private static DEL_V_IIIp _glUniform3iv;
        private static DEL_V_IUiUiUi _glUniform3ui;
        private static DEL_V_IIUip _glUniform3uiv;
        private static DEL_V_IFFFF _glUniform4f;
        private static DEL_V_IIFp _glUniform4fv;
        private static DEL_V_IIIII _glUniform4i;
        private static DEL_V_IIIp _glUniform4iv;
        private static DEL_V_IUiUiUiUi _glUniform4ui;
        private static DEL_V_IIUip _glUniform4uiv;
        private static DEL_V_UiUiUi _glUniformBlockBinding;
        private static DEL_V_IIBFp _glUniformMatrix2fv;
        private static DEL_V_IIBFp _glUniformMatrix2x3fv;
        private static DEL_V_IIBFp _glUniformMatrix2x4fv;
        private static DEL_V_IIBFp _glUniformMatrix3fv;
        private static DEL_V_IIBFp _glUniformMatrix3x2fv;
        private static DEL_V_IIBFp _glUniformMatrix3x4fv;
        private static DEL_V_IIBFp _glUniformMatrix4fv;
        private static DEL_V_IIBFp _glUniformMatrix4x2fv;
        private static DEL_V_IIBFp _glUniformMatrix4x3fv;
        private static DEL_B_I _glUnmapBuffer;
        private static DEL_V_Ui _glUseProgram;
        private static DEL_V_Ui _glValidateProgram;
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
        private static DEL_V_UiSbyp _glVertexAttrib4bv;
        private static DEL_V_UiDDDD _glVertexAttrib4d;
        private static DEL_V_UiDp _glVertexAttrib4dv;
        private static DEL_V_UiFFFF _glVertexAttrib4f;
        private static DEL_V_UiFp _glVertexAttrib4fv;
        private static DEL_V_UiIp _glVertexAttrib4iv;
        private static DEL_V_UiSbyp _glVertexAttrib4Nbv;
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
        private static DEL_V_UiUi _glVertexAttribDivisor;
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
        private static DEL_V_UiSbyp _glVertexAttribI4bv;
        private static DEL_V_UiIIII _glVertexAttribI4i;
        private static DEL_V_UiIp _glVertexAttribI4iv;
        private static DEL_V_UiSp _glVertexAttribI4sv;
        private static DEL_V_UiByp _glVertexAttribI4ubv;
        private static DEL_V_UiUiUiUiUi _glVertexAttribI4ui;
        private static DEL_V_UiUip _glVertexAttribI4uiv;
        private static DEL_V_UiUsp _glVertexAttribI4usv;
        private static DEL_V_UiIIIVp _glVertexAttribIPointer;
        private static DEL_V_UiIBUi _glVertexAttribP1ui;
        private static DEL_V_UiIBUip _glVertexAttribP1uiv;
        private static DEL_V_UiIBUi _glVertexAttribP2ui;
        private static DEL_V_UiIBUip _glVertexAttribP2uiv;
        private static DEL_V_UiIBUi _glVertexAttribP3ui;
        private static DEL_V_UiIBUip _glVertexAttribP3uiv;
        private static DEL_V_UiIBUi _glVertexAttribP4ui;
        private static DEL_V_UiIBUip _glVertexAttribP4uiv;
        private static DEL_V_UiIIBIVp _glVertexAttribPointer;
        private static DEL_V_IUi _glVertexP2ui;
        private static DEL_V_IUip _glVertexP2uiv;
        private static DEL_V_IUi _glVertexP3ui;
        private static DEL_V_IUip _glVertexP3uiv;
        private static DEL_V_IUi _glVertexP4ui;
        private static DEL_V_IUip _glVertexP4uiv;
        private static DEL_V_IIII _glViewport;
        private static DEL_V_PUiUl _glWaitSync;

        public static void LoadFunctions()
        {
            _glActiveTexture = GetFunctionDelegate<DEL_V_I>("glActiveTexture");
            _glAttachShader = GetFunctionDelegate<DEL_V_UiUi>("glAttachShader");
            _glBeginConditionalRender = GetFunctionDelegate<DEL_V_UiI>("glBeginConditionalRender");
            _glBeginQuery = GetFunctionDelegate<DEL_V_IUi>("glBeginQuery");
            _glBeginTransformFeedback = GetFunctionDelegate<DEL_V_I>("glBeginTransformFeedback");
            _glBindAttribLocation = GetFunctionDelegate<DEL_V_UiUiByp>("glBindAttribLocation");
            _glBindBuffer = GetFunctionDelegate<DEL_V_IUi>("glBindBuffer");
            _glBindBufferBase = GetFunctionDelegate<DEL_V_IUiUi>("glBindBufferBase");
            _glBindBufferRange = GetFunctionDelegate<DEL_V_IUiUiPP>("glBindBufferRange");
            _glBindFragDataLocation = GetFunctionDelegate<DEL_V_UiUiByp>("glBindFragDataLocation");
            _glBindFragDataLocationIndexed = GetFunctionDelegate<DEL_V_UiUiUiByp>("glBindFragDataLocationIndexed");
            _glBindFramebuffer = GetFunctionDelegate<DEL_V_IUi>("glBindFramebuffer");
            _glBindRenderbuffer = GetFunctionDelegate<DEL_V_IUi>("glBindRenderbuffer");
            _glBindSampler = GetFunctionDelegate<DEL_V_UiUi>("glBindSampler");
            _glBindTexture = GetFunctionDelegate<DEL_V_IUi>("glBindTexture");
            _glBindVertexArray = GetFunctionDelegate<DEL_V_Ui>("glBindVertexArray");
            _glBlendColor = GetFunctionDelegate<DEL_V_FFFF>("glBlendColor");
            _glBlendEquation = GetFunctionDelegate<DEL_V_I>("glBlendEquation");
            _glBlendEquationSeparate = GetFunctionDelegate<DEL_V_II>("glBlendEquationSeparate");
            _glBlendFunc = GetFunctionDelegate<DEL_V_II>("glBlendFunc");
            _glBlendFuncSeparate = GetFunctionDelegate<DEL_V_IIII>("glBlendFuncSeparate");
            _glBlitFramebuffer = GetFunctionDelegate<DEL_V_IIIIIIIIUiI>("glBlitFramebuffer");
            _glBufferData = GetFunctionDelegate<DEL_V_IPVpI>("glBufferData");
            _glBufferSubData = GetFunctionDelegate<DEL_V_IPPVp>("glBufferSubData");
            _glCheckFramebufferStatus = GetFunctionDelegate<DEL_I_I>("glCheckFramebufferStatus");
            _glClampColor = GetFunctionDelegate<DEL_V_II>("glClampColor");
            _glClear = GetFunctionDelegate<DEL_V_Ui>("glClear");
            _glClearBufferfi = GetFunctionDelegate<DEL_V_IIFI>("glClearBufferfi");
            _glClearBufferfv = GetFunctionDelegate<DEL_V_IIFp>("glClearBufferfv");
            _glClearBufferiv = GetFunctionDelegate<DEL_V_IIIp>("glClearBufferiv");
            _glClearBufferuiv = GetFunctionDelegate<DEL_V_IIUip>("glClearBufferuiv");
            _glClearColor = GetFunctionDelegate<DEL_V_FFFF>("glClearColor");
            _glClearDepth = GetFunctionDelegate<DEL_V_D>("glClearDepth");
            _glClearStencil = GetFunctionDelegate<DEL_V_I>("glClearStencil");
            _glClientWaitSync = GetFunctionDelegate<DEL_I_PUiUl>("glClientWaitSync");
            _glColorMask = GetFunctionDelegate<DEL_V_BBBB>("glColorMask");
            _glColorMaski = GetFunctionDelegate<DEL_V_UiBBBB>("glColorMaski");
            _glColorP3ui = GetFunctionDelegate<DEL_V_IUi>("glColorP3ui");
            _glColorP3uiv = GetFunctionDelegate<DEL_V_IUip>("glColorP3uiv");
            _glColorP4ui = GetFunctionDelegate<DEL_V_IUi>("glColorP4ui");
            _glColorP4uiv = GetFunctionDelegate<DEL_V_IUip>("glColorP4uiv");
            _glCompileShader = GetFunctionDelegate<DEL_V_Ui>("glCompileShader");
            _glCompressedTexImage1D = GetFunctionDelegate<DEL_V_IIIIIIVp>("glCompressedTexImage1D");
            _glCompressedTexImage2D = GetFunctionDelegate<DEL_V_IIIIIIIVp>("glCompressedTexImage2D");
            _glCompressedTexImage3D = GetFunctionDelegate<DEL_V_IIIIIIIIVp>("glCompressedTexImage3D");
            _glCompressedTexSubImage1D = GetFunctionDelegate<DEL_V_IIIIIIVp>("glCompressedTexSubImage1D");
            _glCompressedTexSubImage2D = GetFunctionDelegate<DEL_V_IIIIIIIIVp>("glCompressedTexSubImage2D");
            _glCompressedTexSubImage3D = GetFunctionDelegate<DEL_V_IIIIIIIIIIVp>("glCompressedTexSubImage3D");
            _glCopyBufferSubData = GetFunctionDelegate<DEL_V_IIPPP>("glCopyBufferSubData");
            _glCopyTexImage1D = GetFunctionDelegate<DEL_V_IIIIIII>("glCopyTexImage1D");
            _glCopyTexImage2D = GetFunctionDelegate<DEL_V_IIIIIIII>("glCopyTexImage2D");
            _glCopyTexSubImage1D = GetFunctionDelegate<DEL_V_IIIIII>("glCopyTexSubImage1D");
            _glCopyTexSubImage2D = GetFunctionDelegate<DEL_V_IIIIIIII>("glCopyTexSubImage2D");
            _glCopyTexSubImage3D = GetFunctionDelegate<DEL_V_IIIIIIIII>("glCopyTexSubImage3D");
            _glCreateProgram = GetFunctionDelegate<DEL_Ui_>("glCreateProgram");
            _glCreateShader = GetFunctionDelegate<DEL_Ui_I>("glCreateShader");
            _glCullFace = GetFunctionDelegate<DEL_V_I>("glCullFace");
            _glDeleteBuffers = GetFunctionDelegate<DEL_V_IUip>("glDeleteBuffers");
            _glDeleteFramebuffers = GetFunctionDelegate<DEL_V_IUip>("glDeleteFramebuffers");
            _glDeleteProgram = GetFunctionDelegate<DEL_V_Ui>("glDeleteProgram");
            _glDeleteQueries = GetFunctionDelegate<DEL_V_IUip>("glDeleteQueries");
            _glDeleteRenderbuffers = GetFunctionDelegate<DEL_V_IUip>("glDeleteRenderbuffers");
            _glDeleteSamplers = GetFunctionDelegate<DEL_V_IUip>("glDeleteSamplers");
            _glDeleteShader = GetFunctionDelegate<DEL_V_Ui>("glDeleteShader");
            _glDeleteSync = GetFunctionDelegate<DEL_V_P>("glDeleteSync");
            _glDeleteTextures = GetFunctionDelegate<DEL_V_IUip>("glDeleteTextures");
            _glDeleteVertexArrays = GetFunctionDelegate<DEL_V_IUip>("glDeleteVertexArrays");
            _glDepthFunc = GetFunctionDelegate<DEL_V_I>("glDepthFunc");
            _glDepthMask = GetFunctionDelegate<DEL_V_B>("glDepthMask");
            _glDepthRange = GetFunctionDelegate<DEL_V_DD>("glDepthRange");
            _glDetachShader = GetFunctionDelegate<DEL_V_UiUi>("glDetachShader");
            _glDisable = GetFunctionDelegate<DEL_V_I>("glDisable");
            _glDisablei = GetFunctionDelegate<DEL_V_IUi>("glDisablei");
            _glDisableVertexAttribArray = GetFunctionDelegate<DEL_V_Ui>("glDisableVertexAttribArray");
            _glDrawArrays = GetFunctionDelegate<DEL_V_III>("glDrawArrays");
            _glDrawArraysInstanced = GetFunctionDelegate<DEL_V_IIII>("glDrawArraysInstanced");
            _glDrawBuffer = GetFunctionDelegate<DEL_V_I>("glDrawBuffer");
            _glDrawBuffers = GetFunctionDelegate<DEL_V_IIp>("glDrawBuffers");
            _glDrawElements = GetFunctionDelegate<DEL_V_IIIVp>("glDrawElements");
            _glDrawElementsBaseVertex = GetFunctionDelegate<DEL_V_IIIVpI>("glDrawElementsBaseVertex");
            _glDrawElementsInstanced = GetFunctionDelegate<DEL_V_IIIVpI>("glDrawElementsInstanced");
            _glDrawElementsInstancedBaseVertex = GetFunctionDelegate<DEL_V_IIIVpII>("glDrawElementsInstancedBaseVertex");
            _glDrawRangeElements = GetFunctionDelegate<DEL_V_IUiUiIIVp>("glDrawRangeElements");
            _glDrawRangeElementsBaseVertex = GetFunctionDelegate<DEL_V_IUiUiIIVpI>("glDrawRangeElementsBaseVertex");
            _glEnable = GetFunctionDelegate<DEL_V_I>("glEnable");
            _glEnablei = GetFunctionDelegate<DEL_V_IUi>("glEnablei");
            _glEnableVertexAttribArray = GetFunctionDelegate<DEL_V_Ui>("glEnableVertexAttribArray");
            _glEndConditionalRender = GetFunctionDelegate<DEL_V_>("glEndConditionalRender");
            _glEndQuery = GetFunctionDelegate<DEL_V_I>("glEndQuery");
            _glEndTransformFeedback = GetFunctionDelegate<DEL_V_>("glEndTransformFeedback");
            _glFenceSync = GetFunctionDelegate<DEL_P_IUi>("glFenceSync");
            _glFinish = GetFunctionDelegate<DEL_V_>("glFinish");
            _glFlush = GetFunctionDelegate<DEL_V_>("glFlush");
            _glFlushMappedBufferRange = GetFunctionDelegate<DEL_V_IPP>("glFlushMappedBufferRange");
            _glFramebufferRenderbuffer = GetFunctionDelegate<DEL_V_IIIUi>("glFramebufferRenderbuffer");
            _glFramebufferTexture = GetFunctionDelegate<DEL_V_IIUiI>("glFramebufferTexture");
            _glFramebufferTexture1D = GetFunctionDelegate<DEL_V_IIIUiI>("glFramebufferTexture1D");
            _glFramebufferTexture2D = GetFunctionDelegate<DEL_V_IIIUiI>("glFramebufferTexture2D");
            _glFramebufferTexture3D = GetFunctionDelegate<DEL_V_IIIUiII>("glFramebufferTexture3D");
            _glFramebufferTextureLayer = GetFunctionDelegate<DEL_V_IIUiII>("glFramebufferTextureLayer");
            _glFrontFace = GetFunctionDelegate<DEL_V_I>("glFrontFace");
            _glGenBuffers = GetFunctionDelegate<DEL_V_IUip>("glGenBuffers");
            _glGenerateMipmap = GetFunctionDelegate<DEL_V_I>("glGenerateMipmap");
            _glGenFramebuffers = GetFunctionDelegate<DEL_V_IUip>("glGenFramebuffers");
            _glGenQueries = GetFunctionDelegate<DEL_V_IUip>("glGenQueries");
            _glGenRenderbuffers = GetFunctionDelegate<DEL_V_IUip>("glGenRenderbuffers");
            _glGenSamplers = GetFunctionDelegate<DEL_V_IUip>("glGenSamplers");
            _glGenTextures = GetFunctionDelegate<DEL_V_IUip>("glGenTextures");
            _glGenVertexArrays = GetFunctionDelegate<DEL_V_IUip>("glGenVertexArrays");
            _glGetActiveAttrib = GetFunctionDelegate<DEL_V_UiUiIOiOiOiP>("glGetActiveAttrib");
            _glGetActiveUniform = GetFunctionDelegate<DEL_V_UiUiIOiOiOiP>("glGetActiveUniform");
            _glGetActiveUniformBlockiv = GetFunctionDelegate<DEL_V_UiUiIIp>("glGetActiveUniformBlockiv");
            _glGetActiveUniformBlockName = GetFunctionDelegate<DEL_V_UiUiIIpByp>("glGetActiveUniformBlockName");
            _glGetActiveUniformName = GetFunctionDelegate<DEL_V_UiUiIIpByp>("glGetActiveUniformName");
            _glGetActiveUniformsiv = GetFunctionDelegate<DEL_V_UiIUipIIp>("glGetActiveUniformsiv");
            _glGetAttachedShaders = GetFunctionDelegate<DEL_V_UiIIpUip>("glGetAttachedShaders");
            _glGetAttribLocation = GetFunctionDelegate<DEL_I_UiByp>("glGetAttribLocation");
            _glGetBooleani_v = GetFunctionDelegate<DEL_V_IUiBp>("glGetBooleani_v");
            _glGetBooleanv = GetFunctionDelegate<DEL_V_IBp>("glGetBooleanv");
            _glGetBufferParameteri64v = GetFunctionDelegate<DEL_V_IILp>("glGetBufferParameteri64v");
            _glGetBufferParameteriv = GetFunctionDelegate<DEL_V_IIIp>("glGetBufferParameteriv");
            _glGetBufferPointerv = GetFunctionDelegate<DEL_V_IIOp>("glGetBufferPointerv");
            _glGetBufferSubData = GetFunctionDelegate<DEL_V_IPPVp>("glGetBufferSubData");
            _glGetCompressedTexImage = GetFunctionDelegate<DEL_V_IIVp>("glGetCompressedTexImage");
            _glGetDoublev = GetFunctionDelegate<DEL_V_IDp>("glGetDoublev");
            _glGetError = GetFunctionDelegate<DEL_I_>("glGetError");
            _glGetFloatv = GetFunctionDelegate<DEL_V_IFp>("glGetFloatv");
            _glGetFragDataIndex = GetFunctionDelegate<DEL_I_UiByp>("glGetFragDataIndex");
            _glGetFragDataLocation = GetFunctionDelegate<DEL_I_UiByp>("glGetFragDataLocation");
            _glGetFramebufferAttachmentParameteriv = GetFunctionDelegate<DEL_V_IIIIp>("glGetFramebufferAttachmentParameteriv");
            _glGetInteger64i_v = GetFunctionDelegate<DEL_V_IUiLp>("glGetInteger64i_v");
            _glGetInteger64v = GetFunctionDelegate<DEL_V_ILp>("glGetInteger64v");
            _glGetIntegeri_v = GetFunctionDelegate<DEL_V_IUiIp>("glGetIntegeri_v");
            _glGetIntegerv = GetFunctionDelegate<DEL_V_IIp>("glGetIntegerv");
            _glGetMultisamplefv = GetFunctionDelegate<DEL_V_IUiFp>("glGetMultisamplefv");
            _glGetProgramInfoLog = GetFunctionDelegate<DEL_V_UiIIpByp>("glGetProgramInfoLog");
            _glGetProgramiv = GetFunctionDelegate<DEL_V_UiIIp>("glGetProgramiv");
            _glGetQueryiv = GetFunctionDelegate<DEL_V_IIIp>("glGetQueryiv");
            _glGetQueryObjecti64v = GetFunctionDelegate<DEL_V_UiILp>("glGetQueryObjecti64v");
            _glGetQueryObjectiv = GetFunctionDelegate<DEL_V_UiIIp>("glGetQueryObjectiv");
            _glGetQueryObjectui64v = GetFunctionDelegate<DEL_V_UiIUlp>("glGetQueryObjectui64v");
            _glGetQueryObjectuiv = GetFunctionDelegate<DEL_V_UiIUip>("glGetQueryObjectuiv");
            _glGetRenderbufferParameteriv = GetFunctionDelegate<DEL_V_IIIp>("glGetRenderbufferParameteriv");
            _glGetSamplerParameterfv = GetFunctionDelegate<DEL_V_UiIFp>("glGetSamplerParameterfv");
            _glGetSamplerParameterIiv = GetFunctionDelegate<DEL_V_UiIIp>("glGetSamplerParameterIiv");
            _glGetSamplerParameterIuiv = GetFunctionDelegate<DEL_V_UiIUip>("glGetSamplerParameterIuiv");
            _glGetSamplerParameteriv = GetFunctionDelegate<DEL_V_UiIIp>("glGetSamplerParameteriv");
            _glGetShaderInfoLog = GetFunctionDelegate<DEL_V_UiIIpByp>("glGetShaderInfoLog");
            _glGetShaderiv = GetFunctionDelegate<DEL_V_UiIIp>("glGetShaderiv");
            _glGetShaderSource = GetFunctionDelegate<DEL_V_UiIIpByp>("glGetShaderSource");
            _glGetString = GetFunctionDelegate<DEL_Byp_I>("glGetString");
            _glGetStringi = GetFunctionDelegate<DEL_Byp_IUi>("glGetStringi");
            _glGetSynciv = GetFunctionDelegate<DEL_V_PIIIpIp>("glGetSynciv");
            _glGetTexImage = GetFunctionDelegate<DEL_V_IIIIVp>("glGetTexImage");
            _glGetTexLevelParameterfv = GetFunctionDelegate<DEL_V_IIIFp>("glGetTexLevelParameterfv");
            _glGetTexLevelParameteriv = GetFunctionDelegate<DEL_V_IIIIp>("glGetTexLevelParameteriv");
            _glGetTexParameterfv = GetFunctionDelegate<DEL_V_IIFp>("glGetTexParameterfv");
            _glGetTexParameterIiv = GetFunctionDelegate<DEL_V_IIIp>("glGetTexParameterIiv");
            _glGetTexParameterIuiv = GetFunctionDelegate<DEL_V_IIUip>("glGetTexParameterIuiv");
            _glGetTexParameteriv = GetFunctionDelegate<DEL_V_IIIp>("glGetTexParameteriv");
            _glGetTransformFeedbackVarying = GetFunctionDelegate<DEL_V_UiUiIOiOiOiP>("glGetTransformFeedbackVarying");
            _glGetUniformBlockIndex = GetFunctionDelegate<DEL_Ui_UiByp>("glGetUniformBlockIndex");
            _glGetUniformfv = GetFunctionDelegate<DEL_V_UiIFp>("glGetUniformfv");
            _glGetUniformIndices = GetFunctionDelegate<DEL_V_UiIByppUip>("glGetUniformIndices");
            _glGetUniformiv = GetFunctionDelegate<DEL_V_UiIIp>("glGetUniformiv");
            _glGetUniformLocation = GetFunctionDelegate<DEL_I_UiByp>("glGetUniformLocation");
            _glGetUniformuiv = GetFunctionDelegate<DEL_V_UiIUip>("glGetUniformuiv");
            _glGetVertexAttribdv = GetFunctionDelegate<DEL_V_UiIDp>("glGetVertexAttribdv");
            _glGetVertexAttribfv = GetFunctionDelegate<DEL_V_UiIFp>("glGetVertexAttribfv");
            _glGetVertexAttribIiv = GetFunctionDelegate<DEL_V_UiIIp>("glGetVertexAttribIiv");
            _glGetVertexAttribIuiv = GetFunctionDelegate<DEL_V_UiIUip>("glGetVertexAttribIuiv");
            _glGetVertexAttribiv = GetFunctionDelegate<DEL_V_UiIIp>("glGetVertexAttribiv");
            _glGetVertexAttribPointerv = GetFunctionDelegate<DEL_V_UiIOp>("glGetVertexAttribPointerv");
            _glHint = GetFunctionDelegate<DEL_V_II>("glHint");
            _glIsBuffer = GetFunctionDelegate<DEL_B_Ui>("glIsBuffer");
            _glIsEnabled = GetFunctionDelegate<DEL_B_I>("glIsEnabled");
            _glIsEnabledi = GetFunctionDelegate<DEL_B_IUi>("glIsEnabledi");
            _glIsFramebuffer = GetFunctionDelegate<DEL_B_Ui>("glIsFramebuffer");
            _glIsProgram = GetFunctionDelegate<DEL_B_Ui>("glIsProgram");
            _glIsQuery = GetFunctionDelegate<DEL_B_Ui>("glIsQuery");
            _glIsRenderbuffer = GetFunctionDelegate<DEL_B_Ui>("glIsRenderbuffer");
            _glIsSampler = GetFunctionDelegate<DEL_B_Ui>("glIsSampler");
            _glIsShader = GetFunctionDelegate<DEL_B_Ui>("glIsShader");
            _glIsSync = GetFunctionDelegate<DEL_B_P>("glIsSync");
            _glIsTexture = GetFunctionDelegate<DEL_B_Ui>("glIsTexture");
            _glIsVertexArray = GetFunctionDelegate<DEL_B_Ui>("glIsVertexArray");
            _glLineWidth = GetFunctionDelegate<DEL_V_F>("glLineWidth");
            _glLinkProgram = GetFunctionDelegate<DEL_V_Ui>("glLinkProgram");
            _glLogicOp = GetFunctionDelegate<DEL_V_I>("glLogicOp");
            _glMapBuffer = GetFunctionDelegate<DEL_Vp_II>("glMapBuffer");
            _glMapBufferRange = GetFunctionDelegate<DEL_Vp_IPPUi>("glMapBufferRange");
            _glMultiDrawArrays = GetFunctionDelegate<DEL_V_IIpIpI>("glMultiDrawArrays");
            _glMultiDrawElements = GetFunctionDelegate<DEL_V_IIpIVppI>("glMultiDrawElements");
            _glMultiDrawElementsBaseVertex = GetFunctionDelegate<DEL_V_IIpIVppIIp>("glMultiDrawElementsBaseVertex");
            _glMultiTexCoordP1ui = GetFunctionDelegate<DEL_V_IIUi>("glMultiTexCoordP1ui");
            _glMultiTexCoordP1uiv = GetFunctionDelegate<DEL_V_IIUip>("glMultiTexCoordP1uiv");
            _glMultiTexCoordP2ui = GetFunctionDelegate<DEL_V_IIUi>("glMultiTexCoordP2ui");
            _glMultiTexCoordP2uiv = GetFunctionDelegate<DEL_V_IIUip>("glMultiTexCoordP2uiv");
            _glMultiTexCoordP3ui = GetFunctionDelegate<DEL_V_IIUi>("glMultiTexCoordP3ui");
            _glMultiTexCoordP3uiv = GetFunctionDelegate<DEL_V_IIUip>("glMultiTexCoordP3uiv");
            _glMultiTexCoordP4ui = GetFunctionDelegate<DEL_V_IIUi>("glMultiTexCoordP4ui");
            _glMultiTexCoordP4uiv = GetFunctionDelegate<DEL_V_IIUip>("glMultiTexCoordP4uiv");
            _glNormalP3ui = GetFunctionDelegate<DEL_V_IUi>("glNormalP3ui");
            _glNormalP3uiv = GetFunctionDelegate<DEL_V_IUip>("glNormalP3uiv");
            _glPixelStoref = GetFunctionDelegate<DEL_V_IF>("glPixelStoref");
            _glPixelStorei = GetFunctionDelegate<DEL_V_II>("glPixelStorei");
            _glPointParameterf = GetFunctionDelegate<DEL_V_IF>("glPointParameterf");
            _glPointParameterfv = GetFunctionDelegate<DEL_V_IFp>("glPointParameterfv");
            _glPointParameteri = GetFunctionDelegate<DEL_V_II>("glPointParameteri");
            _glPointParameteriv = GetFunctionDelegate<DEL_V_IIp>("glPointParameteriv");
            _glPointSize = GetFunctionDelegate<DEL_V_F>("glPointSize");
            _glPolygonMode = GetFunctionDelegate<DEL_V_II>("glPolygonMode");
            _glPolygonOffset = GetFunctionDelegate<DEL_V_FF>("glPolygonOffset");
            _glPrimitiveRestartIndex = GetFunctionDelegate<DEL_V_Ui>("glPrimitiveRestartIndex");
            _glProvokingVertex = GetFunctionDelegate<DEL_V_I>("glProvokingVertex");
            _glQueryCounter = GetFunctionDelegate<DEL_V_UiI>("glQueryCounter");
            _glReadBuffer = GetFunctionDelegate<DEL_V_I>("glReadBuffer");
            _glReadPixels = GetFunctionDelegate<DEL_V_IIIIIIVp>("glReadPixels");
            _glRenderbufferStorage = GetFunctionDelegate<DEL_V_IIII>("glRenderbufferStorage");
            _glRenderbufferStorageMultisample = GetFunctionDelegate<DEL_V_IIIII>("glRenderbufferStorageMultisample");
            _glSampleCoverage = GetFunctionDelegate<DEL_V_FB>("glSampleCoverage");
            _glSampleMaski = GetFunctionDelegate<DEL_V_UiUi>("glSampleMaski");
            _glSamplerParameterf = GetFunctionDelegate<DEL_V_UiIF>("glSamplerParameterf");
            _glSamplerParameterfv = GetFunctionDelegate<DEL_V_UiIFp>("glSamplerParameterfv");
            _glSamplerParameteri = GetFunctionDelegate<DEL_V_UiII>("glSamplerParameteri");
            _glSamplerParameterIiv = GetFunctionDelegate<DEL_V_UiIIp>("glSamplerParameterIiv");
            _glSamplerParameterIuiv = GetFunctionDelegate<DEL_V_UiIUip>("glSamplerParameterIuiv");
            _glSamplerParameteriv = GetFunctionDelegate<DEL_V_UiIIp>("glSamplerParameteriv");
            _glScissor = GetFunctionDelegate<DEL_V_IIII>("glScissor");
            _glSecondaryColorP3ui = GetFunctionDelegate<DEL_V_IUi>("glSecondaryColorP3ui");
            _glSecondaryColorP3uiv = GetFunctionDelegate<DEL_V_IUip>("glSecondaryColorP3uiv");
            _glShaderSource = GetFunctionDelegate<DEL_V_UiIByppIp>("glShaderSource");
            _glStencilFunc = GetFunctionDelegate<DEL_V_IIUi>("glStencilFunc");
            _glStencilFuncSeparate = GetFunctionDelegate<DEL_V_IIIUi>("glStencilFuncSeparate");
            _glStencilMask = GetFunctionDelegate<DEL_V_Ui>("glStencilMask");
            _glStencilMaskSeparate = GetFunctionDelegate<DEL_V_IUi>("glStencilMaskSeparate");
            _glStencilOp = GetFunctionDelegate<DEL_V_III>("glStencilOp");
            _glStencilOpSeparate = GetFunctionDelegate<DEL_V_IIII>("glStencilOpSeparate");
            _glTexBuffer = GetFunctionDelegate<DEL_V_IIUi>("glTexBuffer");
            _glTexCoordP1ui = GetFunctionDelegate<DEL_V_IUi>("glTexCoordP1ui");
            _glTexCoordP1uiv = GetFunctionDelegate<DEL_V_IUip>("glTexCoordP1uiv");
            _glTexCoordP2ui = GetFunctionDelegate<DEL_V_IUi>("glTexCoordP2ui");
            _glTexCoordP2uiv = GetFunctionDelegate<DEL_V_IUip>("glTexCoordP2uiv");
            _glTexCoordP3ui = GetFunctionDelegate<DEL_V_IUi>("glTexCoordP3ui");
            _glTexCoordP3uiv = GetFunctionDelegate<DEL_V_IUip>("glTexCoordP3uiv");
            _glTexCoordP4ui = GetFunctionDelegate<DEL_V_IUi>("glTexCoordP4ui");
            _glTexCoordP4uiv = GetFunctionDelegate<DEL_V_IUip>("glTexCoordP4uiv");
            _glTexImage1D = GetFunctionDelegate<DEL_V_IIIIIIIVp>("glTexImage1D");
            _glTexImage2D = GetFunctionDelegate<DEL_V_IIIIIIIIVp>("glTexImage2D");
            _glTexImage2DMultisample = GetFunctionDelegate<DEL_V_IIIIIB>("glTexImage2DMultisample");
            _glTexImage3D = GetFunctionDelegate<DEL_V_IIIIIIIIIVp>("glTexImage3D");
            _glTexImage3DMultisample = GetFunctionDelegate<DEL_V_IIIIIIB>("glTexImage3DMultisample");
            _glTexParameterf = GetFunctionDelegate<DEL_V_IIF>("glTexParameterf");
            _glTexParameterfv = GetFunctionDelegate<DEL_V_IIFp>("glTexParameterfv");
            _glTexParameteri = GetFunctionDelegate<DEL_V_III>("glTexParameteri");
            _glTexParameterIiv = GetFunctionDelegate<DEL_V_IIIp>("glTexParameterIiv");
            _glTexParameterIuiv = GetFunctionDelegate<DEL_V_IIUip>("glTexParameterIuiv");
            _glTexParameteriv = GetFunctionDelegate<DEL_V_IIIp>("glTexParameteriv");
            _glTexSubImage1D = GetFunctionDelegate<DEL_V_IIIIIIVp>("glTexSubImage1D");
            _glTexSubImage2D = GetFunctionDelegate<DEL_V_IIIIIIIIVp>("glTexSubImage2D");
            _glTexSubImage3D = GetFunctionDelegate<DEL_V_IIIIIIIIIIVp>("glTexSubImage3D");
            _glTransformFeedbackVaryings = GetFunctionDelegate<DEL_V_UiIByppI>("glTransformFeedbackVaryings");
            _glUniform1f = GetFunctionDelegate<DEL_V_IF>("glUniform1f");
            _glUniform1fv = GetFunctionDelegate<DEL_V_IIFp>("glUniform1fv");
            _glUniform1i = GetFunctionDelegate<DEL_V_II>("glUniform1i");
            _glUniform1iv = GetFunctionDelegate<DEL_V_IIIp>("glUniform1iv");
            _glUniform1ui = GetFunctionDelegate<DEL_V_IUi>("glUniform1ui");
            _glUniform1uiv = GetFunctionDelegate<DEL_V_IIUip>("glUniform1uiv");
            _glUniform2f = GetFunctionDelegate<DEL_V_IFF>("glUniform2f");
            _glUniform2fv = GetFunctionDelegate<DEL_V_IIFp>("glUniform2fv");
            _glUniform2i = GetFunctionDelegate<DEL_V_III>("glUniform2i");
            _glUniform2iv = GetFunctionDelegate<DEL_V_IIIp>("glUniform2iv");
            _glUniform2ui = GetFunctionDelegate<DEL_V_IUiUi>("glUniform2ui");
            _glUniform2uiv = GetFunctionDelegate<DEL_V_IIUip>("glUniform2uiv");
            _glUniform3f = GetFunctionDelegate<DEL_V_IFFF>("glUniform3f");
            _glUniform3fv = GetFunctionDelegate<DEL_V_IIFp>("glUniform3fv");
            _glUniform3i = GetFunctionDelegate<DEL_V_IIII>("glUniform3i");
            _glUniform3iv = GetFunctionDelegate<DEL_V_IIIp>("glUniform3iv");
            _glUniform3ui = GetFunctionDelegate<DEL_V_IUiUiUi>("glUniform3ui");
            _glUniform3uiv = GetFunctionDelegate<DEL_V_IIUip>("glUniform3uiv");
            _glUniform4f = GetFunctionDelegate<DEL_V_IFFFF>("glUniform4f");
            _glUniform4fv = GetFunctionDelegate<DEL_V_IIFp>("glUniform4fv");
            _glUniform4i = GetFunctionDelegate<DEL_V_IIIII>("glUniform4i");
            _glUniform4iv = GetFunctionDelegate<DEL_V_IIIp>("glUniform4iv");
            _glUniform4ui = GetFunctionDelegate<DEL_V_IUiUiUiUi>("glUniform4ui");
            _glUniform4uiv = GetFunctionDelegate<DEL_V_IIUip>("glUniform4uiv");
            _glUniformBlockBinding = GetFunctionDelegate<DEL_V_UiUiUi>("glUniformBlockBinding");
            _glUniformMatrix2fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix2fv");
            _glUniformMatrix2x3fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix2x3fv");
            _glUniformMatrix2x4fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix2x4fv");
            _glUniformMatrix3fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix3fv");
            _glUniformMatrix3x2fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix3x2fv");
            _glUniformMatrix3x4fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix3x4fv");
            _glUniformMatrix4fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix4fv");
            _glUniformMatrix4x2fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix4x2fv");
            _glUniformMatrix4x3fv = GetFunctionDelegate<DEL_V_IIBFp>("glUniformMatrix4x3fv");
            _glUnmapBuffer = GetFunctionDelegate<DEL_B_I>("glUnmapBuffer");
            _glUseProgram = GetFunctionDelegate<DEL_V_Ui>("glUseProgram");
            _glValidateProgram = GetFunctionDelegate<DEL_V_Ui>("glValidateProgram");
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
            _glVertexAttrib4bv = GetFunctionDelegate<DEL_V_UiSbyp>("glVertexAttrib4bv");
            _glVertexAttrib4d = GetFunctionDelegate<DEL_V_UiDDDD>("glVertexAttrib4d");
            _glVertexAttrib4dv = GetFunctionDelegate<DEL_V_UiDp>("glVertexAttrib4dv");
            _glVertexAttrib4f = GetFunctionDelegate<DEL_V_UiFFFF>("glVertexAttrib4f");
            _glVertexAttrib4fv = GetFunctionDelegate<DEL_V_UiFp>("glVertexAttrib4fv");
            _glVertexAttrib4iv = GetFunctionDelegate<DEL_V_UiIp>("glVertexAttrib4iv");
            _glVertexAttrib4Nbv = GetFunctionDelegate<DEL_V_UiSbyp>("glVertexAttrib4Nbv");
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
            _glVertexAttribDivisor = GetFunctionDelegate<DEL_V_UiUi>("glVertexAttribDivisor");
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
            _glVertexAttribI4bv = GetFunctionDelegate<DEL_V_UiSbyp>("glVertexAttribI4bv");
            _glVertexAttribI4i = GetFunctionDelegate<DEL_V_UiIIII>("glVertexAttribI4i");
            _glVertexAttribI4iv = GetFunctionDelegate<DEL_V_UiIp>("glVertexAttribI4iv");
            _glVertexAttribI4sv = GetFunctionDelegate<DEL_V_UiSp>("glVertexAttribI4sv");
            _glVertexAttribI4ubv = GetFunctionDelegate<DEL_V_UiByp>("glVertexAttribI4ubv");
            _glVertexAttribI4ui = GetFunctionDelegate<DEL_V_UiUiUiUiUi>("glVertexAttribI4ui");
            _glVertexAttribI4uiv = GetFunctionDelegate<DEL_V_UiUip>("glVertexAttribI4uiv");
            _glVertexAttribI4usv = GetFunctionDelegate<DEL_V_UiUsp>("glVertexAttribI4usv");
            _glVertexAttribIPointer = GetFunctionDelegate<DEL_V_UiIIIVp>("glVertexAttribIPointer");
            _glVertexAttribP1ui = GetFunctionDelegate<DEL_V_UiIBUi>("glVertexAttribP1ui");
            _glVertexAttribP1uiv = GetFunctionDelegate<DEL_V_UiIBUip>("glVertexAttribP1uiv");
            _glVertexAttribP2ui = GetFunctionDelegate<DEL_V_UiIBUi>("glVertexAttribP2ui");
            _glVertexAttribP2uiv = GetFunctionDelegate<DEL_V_UiIBUip>("glVertexAttribP2uiv");
            _glVertexAttribP3ui = GetFunctionDelegate<DEL_V_UiIBUi>("glVertexAttribP3ui");
            _glVertexAttribP3uiv = GetFunctionDelegate<DEL_V_UiIBUip>("glVertexAttribP3uiv");
            _glVertexAttribP4ui = GetFunctionDelegate<DEL_V_UiIBUi>("glVertexAttribP4ui");
            _glVertexAttribP4uiv = GetFunctionDelegate<DEL_V_UiIBUip>("glVertexAttribP4uiv");
            _glVertexAttribPointer = GetFunctionDelegate<DEL_V_UiIIBIVp>("glVertexAttribPointer");
            _glVertexP2ui = GetFunctionDelegate<DEL_V_IUi>("glVertexP2ui");
            _glVertexP2uiv = GetFunctionDelegate<DEL_V_IUip>("glVertexP2uiv");
            _glVertexP3ui = GetFunctionDelegate<DEL_V_IUi>("glVertexP3ui");
            _glVertexP3uiv = GetFunctionDelegate<DEL_V_IUip>("glVertexP3uiv");
            _glVertexP4ui = GetFunctionDelegate<DEL_V_IUi>("glVertexP4ui");
            _glVertexP4uiv = GetFunctionDelegate<DEL_V_IUip>("glVertexP4uiv");
            _glViewport = GetFunctionDelegate<DEL_V_IIII>("glViewport");
            _glWaitSync = GetFunctionDelegate<DEL_V_PUiUl>("glWaitSync");
        }

        /*public static int GenBuffer()
        {
            
        }

        public static int CreateShader(int type) // enum?
        {
            
        } 

        public static void ShaderSource(int shader, string code)
        {

        }

        public static void CompileShader(int shader)
        {

        }

        public static int GetShader(int shader, int compileStatus) // enum?
        {

        }

        public static void AttachShader(int program, int shader)
        {
            
        }

        public static void DetachShader(int program, int shader)
        {

        }

        public static void DeleteShader(int shader)
        {

        }

        public static void LinkProgram(int program)
        {

        }

        public static void UseProgram(int program)
        {

        }

        public static int GetAttributeLocation(int program, string name)
        {

        }

        public static int GetUniformLocation(int program, string name)
        {
            
        }

        public static int GetUniformBlockIndex(int program, string name)
        {

        }

        public static void UniformBlockBinding(int program, int index, int binding)
        {

        }*/

        public static void ShaderStorageBlockBinding(int program, int index, int binding)
        {

        }

        /*public static void ActiveTexture(int index) // enum?
        {
            
        }

        public static void BindTexture(int target, int texture) // enum?
        {
            
        }*/

        public static void BindImageTexture(int unit, int texture, int level, bool layerd, int layer, int access, int format) // enum?
        {

        }

        /*public static void GenerateTextureMipmap(int texture)
        {
            
        }*/

        public static void ClearTexImage(int texture, int level, int format, int type, IntPtr data) // enum?
        {

        }

        public static void TexImage1D(int target, int level, int internalFormat, int width, int border, int format, int type, int type2, byte[] pixels) // enum?
        {

        }

        public static void TexImage2D(int target, int level, int internalFormat, int width, int height, int border, int format, int type, int type2, byte[] pixels) // enum?
        {

        }

        public static void TexImage3D(int target, int level, int internalFormat, int width, int height, int depth, int border, int format, int type, int type2, byte[] pixels) // enum?
        {

        }

        public static void TexStorage3D(int target, int level, int format, int width, int height, int depth) // enum?
        {

        }

        public static void TexSubImage3D(int target, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, IntPtr pixels) // enum?
        {

        }

        public static void TexParameter(int target, int level, int param) // enum?
        {

        }

        public static void DeleteTexture(int texture)
        {
            
        }

        public static byte[] ReadPixels(int x, int y, int width, int height, int format, int type) // enum?
        {
            return null;
        }

        public static void DrawArrays(int type, int index, int count) // enum?
        {

        }

        public static void DrawBuffer(int buffer)
        {

        }

        public static void DrawElements(int mode, int count, int type, IntPtr indices)
        {

        }

        public static void EnableVertexAttribArray(int index)
        {

        }

        public static void VertexAttribPointer(int index, int size, int type, bool normalize, int stride, int offset) // enum?
        {

        }

        public static int GenVertexArray()
        {
            return 0;
        }

        public static void BindVertexArray(int array)
        {

        }

        public static void DeleteVertexArray(int array)
        {

        }

        public static void BindBuffer(int type, int buffer)
        {

        }

        public static void BindBufferBase(int type, int binding, int buffer)
        {

        }

        public static void DeleteBuffer(int buffer)
        {

        }

        public static void FrameBufferTexture(int target, int attachment, int texture, int level)
        {

        }

        public static void FrameBufferRenderBuffer(int target, int attachment, int renderTarget, int renderBuffer)
        {

        }

        public static int CheckFrameBufferStatus(int target)
        {
            return 0;
        }

        public static void BindFrameBuffer(int target, int buffer)
        {

        }

        public static void DeleteFrameBuffer(int buffer)
        {

        }

        public static void ReadBuffer(int source)
        {

        }

        public static int GenRenderBuffer()
        {
            return 0;
        }

        public static void BindRenderBuffer(int target, int buffer)
        {

        }

        public static void RenderBufferStorage(int target, int format, int width, int height)
        {

        }

        public static void DeleteRenderBuffer(int buffer)
        {

        }

        public static void BufferData(int target, int size, IntPtr data, int usage)
        {

        }

        public static void Enable(int flag)
        {

        }

        public static void Disable(int flag)
        {

        }

        public static void Clear(int flag)
        {

        }

        public static void ClearColor(int r, int g, int b, int a)
        {

        }

        public static void CullFace(int flag)
        {

        }

        public static void DepthFunc(int flag)
        {

        }

        public static void StencilFunc(int func, int reference, int mask)
        {

        }

        public static void PixelStore(int parameter, int param)
        {

        }

        public static void DispatchCompute(int nGroupsX, int nGroupsY, int nGroupsZ)
        {

        }

        public static void MemoryBarrier(int flag)
        {

        }

        public static int GetInteger(string name)
        {
            return 0;
        }

        public static float GetFloat(int name)
        {
            return 0;
        }

        public static void ActiveTexture(int texture) => _glActiveTexture(texture);
        public static void AttachShader(uint program, uint shader) => _glAttachShader(program, shader);
        public static void BeginConditionalRender(uint id, int mode) => _glBeginConditionalRender(id, mode);
        public static void BeginQuery(int target, uint id) => _glBeginQuery(target, id);
        public static void BeginTransformFeedback(int primitiveMode) => _glBeginTransformFeedback(primitiveMode);
        public static void BindBuffer(int target, uint buffer) => _glBindBuffer(target, buffer);
        public static void BindBufferBase(int target, uint index, uint buffer) => _glBindBufferBase(target, index, buffer);
        public static void BindBufferRange(int target, uint index, uint buffer, int offset, int size) => _glBindBufferRange(target, index, buffer, new IntPtr(offset), new IntPtr(size));
        public static void BindBufferRange(int target, uint index, uint buffer, long offset, long size) => _glBindBufferRange(target, index, buffer, new IntPtr(offset), new IntPtr(size));
        public static void BindFramebuffer(uint framebuffer) => _glBindFramebuffer(GL_FRAMEBUFFER, framebuffer);
        public static void BindFramebuffer(int target, uint framebuffer) => _glBindFramebuffer(target, framebuffer);
        public static void BindRenderbuffer(uint renderbuffer) => _glBindRenderbuffer(GL_RENDERBUFFER, renderbuffer);
        public static void BindSampler(uint unit, uint sampler) => _glBindSampler(unit, sampler);
        public static void BindTexture(int target, uint texture) => _glBindTexture(target, texture);
        public static void BindVertexArray(uint array) => _glBindVertexArray(array);
        public static void BlendColor(float red, float green, float blue, float alpha) => _glBlendColor(red, green, blue, alpha);
        public static void BlendEquation(int mode) => _glBlendEquation(mode);
        public static void BlendEquationSeparate(int modeRGB, int modeAlpha) => _glBlendEquationSeparate(modeRGB, modeAlpha);
        public static void BlendFunc(int srcFactor, int dstFactor) => _glBlendFunc(srcFactor, dstFactor);
        public static void BlendFuncSeparate(int sFactorRgb, int dFactorRgb, int sFactorAlpha, int dFactorAlpha) => _glBlendFuncSeparate(sFactorRgb, dFactorRgb, sFactorAlpha, dFactorAlpha);
        public static void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter) => _glBlitFramebuffer(srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
        //public static void BufferData(int target, int size, IntPtr data, int usage) => _glBufferData(target, new IntPtr(size), data.ToPointer(), usage);
        public static void BufferData(int target, int size, void* data, int usage) => _glBufferData(target, new IntPtr(size), data, usage);
        public static void BufferSubData(int target, int offset, int size, IntPtr data) => _glBufferSubData(target, new IntPtr(offset), new IntPtr(size), data.ToPointer());
        public static void BufferSubData(int target, long offset, long size, IntPtr data) => _glBufferSubData(target, new IntPtr(offset), new IntPtr(size), data.ToPointer());
        public static void BufferSubData(int target, int offset, int size, void* data) => _glBufferSubData(target, new IntPtr(offset), new IntPtr(size), data);
        public static void BufferSubData(int target, long offset, long size, void* data) => _glBufferSubData(target, new IntPtr(offset), new IntPtr(size), data);
        public static int CheckFramebufferStatus(int target) => _glCheckFramebufferStatus(target);
        public static void ClampColor(bool clamp) => _glClampColor(GL_CLAMP_READ_COLOR, clamp ? 1 : 0);
        public static void Clear(uint mask) => _glClear(mask);
        public static void ClearBufferfi(int buffer, int drawbuffer, float depth, int stencil) => _glClearBufferfi(buffer, drawbuffer, depth, stencil);
        public static void ClearBufferfv(int buffer, int drawbuffer, float* value) => _glClearBufferfv(buffer, drawbuffer, value);
        public static void ClearBufferiv(int buffer, int drawbuffer, int* value) => _glClearBufferiv(buffer, drawbuffer, value);
        public static void ClearBufferuiv(int buffer, int drawbuffer, uint* value) => _glClearBufferuiv(buffer, drawbuffer, value);
        public static void ClearColor(float red, float green, float blue, float alpha) => _glClearColor(red, green, blue, alpha);
        public static void ClearDepth(double depth) => _glClearDepth(depth);
        public static void ClearStencil(int index) => _glClearStencil(index);
        public static int ClientWaitSync(IntPtr sync, uint flags, ulong timeout) => _glClientWaitSync(sync, flags, timeout);
        public static void ColorMask(bool red, bool green, bool blue, bool alpha) => _glColorMask(red, green, blue, alpha);
        public static void ColorMaski(uint index, bool red, bool green, bool blue, bool alpha) => _glColorMaski(index, red, green, blue, alpha);
        public static void ColorP3ui(int type, uint color) => _glColorP3ui(type, color);
        public static void ColorP3uiv(int type, uint* color) => _glColorP3uiv(type, color);
        public static void ColorP4ui(int type, uint color) => _glColorP4ui(type, color);
        public static void ColorP4uiv(int type, uint* color) => _glColorP4uiv(type, color);
        public static void CompileShader(uint shader) => _glCompileShader(shader);
        public static void CompressedTexImage1D(int target, int level, int internalFormat, int width, int border, int imageSize, IntPtr data) => _glCompressedTexImage1D(target, level, internalFormat, width, border, imageSize, data.ToPointer());
        public static void CompressedTexImage1D(int target, int level, int internalFormat, int width, int border, int imageSize, void* data) => _glCompressedTexImage1D(target, level, internalFormat, width, border, imageSize, data);
        public static void CompressedTexImage2D(int target, int level, int internalFormat, int width, int height, int border, int imageSize, IntPtr data) => _glCompressedTexImage2D(target, level, internalFormat, width, height, border, imageSize, data.ToPointer());
        public static void CompressedTexImage2D(int target, int level, int internalFormat, int width, int height, int border, int imageSize, void* data) => _glCompressedTexImage2D(target, level, internalFormat, width, height, border, imageSize, data);
        public static void CompressedTexImage3D(int target, int level, int internalFormat, int width, int height, int depth, int border, int imageSize, IntPtr data) => _glCompressedTexImage3D(target, level, internalFormat, width, height, depth, border, imageSize, data.ToPointer());
        public static void CompressedTexImage3D(int target, int level, int internalFormat, int width, int height, int depth, int border, int imageSize, void* data) => _glCompressedTexImage3D(target, level, internalFormat, width, height, depth, border, imageSize, data);
        public static void CompressedTexSubImage1D(int target, int level, int xOffset, int width, int format, int imageSize, IntPtr data) => _glCompressedTexSubImage1D(target, level, xOffset, width, format, imageSize, data.ToPointer());
        public static void CompressedTexSubImage1D(int target, int level, int xOffset, int width, int format, int imageSize, void* data) => _glCompressedTexSubImage1D(target, level, xOffset, width, format, imageSize, data);
        public static void CompressedTexSubImage2D(int target, int level, int xOffset, int yOffset, int width, int height, int format, int imageSize, IntPtr data) => _glCompressedTexSubImage2D(target, level, xOffset, yOffset, width, height, format, imageSize, data.ToPointer());
        public static void CompressedTexSubImage2D(int target, int level, int xOffset, int yOffset, int width, int height, int format, int imageSize, void* data) => _glCompressedTexSubImage2D(target, level, xOffset, yOffset, width, height, format, imageSize, data);
        public static void CompressedTexSubImage3D(int target, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int imageSize, IntPtr data) => _glCompressedTexSubImage3D(target, level, xOffset, yOffset, zOffset, width, height, depth, format, imageSize, data.ToPointer());
        public static void CompressedTexSubImage3D(int target, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int imageSize, void* data) => _glCompressedTexSubImage3D(target, level, xOffset, yOffset, zOffset, width, height, depth, format, imageSize, data);
        public static void CopyBufferSubData(int readTarget, int writeTarget, int readOffset, int writeOffset, int size) => _glCopyBufferSubData(readTarget, writeTarget, new IntPtr(readOffset), new IntPtr(writeOffset), new IntPtr(size));
        public static void CopyBufferSubData(int readTarget, int writeTarget, long readOffset, long writeOffset, long size) => _glCopyBufferSubData(readTarget, writeTarget, new IntPtr(readOffset), new IntPtr(writeOffset), new IntPtr(size));
        public static void CopyTexImage1D(int target, int level, int internalFormat, int x, int y, int width, int border) => _glCopyTexImage1D(target, level, internalFormat, x, y, width, border);
        public static void CopyTexImage2D(int target, int level, int internalFormat, int x, int y, int width, int height, int border) => _glCopyTexImage2D(target, level, internalFormat, x, y, width, height, border);
        public static void CopyTexSubImage1D(int target, int level, int xOffset, int x, int y, int width) => _glCopyTexSubImage1D(target, level, xOffset, x, y, width);
        public static void CopyTexSubImage2D(int target, int level, int xOffset, int yOffset, int x, int y, int width, int height) => _glCopyTexSubImage2D(target, level, xOffset, yOffset, x, y, width, height);
        public static void CopyTexSubImage3D(int target, int level, int xOffset, int yOffset, int zOffset, int x, int y, int width, int height) => _glCopyTexSubImage3D(target, level, xOffset, yOffset, zOffset, x, y, width, height);
        public static uint CreateProgram() => _glCreateProgram();
        public static uint CreateShader(int type) => _glCreateShader(type);
        //public static void CullFace(int mode) => _glCullFace(mode);
        public static void DeleteBuffer(uint buffer) => _glDeleteBuffers(1, &buffer);
        public static void DeleteBuffers(int n, uint* buffers) => _glDeleteBuffers(n, buffers);
        public static void DeleteFramebuffers(int n, uint* buffers) => _glDeleteFramebuffers(n, buffers);
        public static void DeleteFramebuffer(uint buffer) => _glDeleteFramebuffers(1, &buffer);
        public static void DeleteProgram(uint program) => _glDeleteProgram(program);
        public static void DeleteQueries(int n, uint* ids) => _glDeleteQueries(n, ids);
        public static void DeleteQuery(uint id) => _glDeleteQueries(1, &id);
        public static void DeleteRenderbuffer(uint renderbuffer) => _glDeleteRenderbuffers(1, &renderbuffer);
        public static void DeleteRenderbuffers(int n, uint* buffers) => _glDeleteRenderbuffers(n, buffers);
        public static void DeleteSamplers(int count, uint* samplers) => _glDeleteSamplers(count, samplers);
        public static void DeleteSampler(uint sampler) => _glDeleteSamplers(1, &sampler);
        public static void DeleteShader(uint shader) => _glDeleteShader(shader);
        public static void DeleteSync(IntPtr sync) => _glDeleteSync(sync);
        public static void DeleteTextures(int n, uint* textures) => _glDeleteTextures(n, textures);
        public static void DeleteTexture(uint texture) => _glDeleteTextures(1, &texture);
        public static void DeleteVertexArrays(int n, uint* arrays) => _glDeleteVertexArrays(n, arrays);
        public static void DeleteVertexArray(uint array) => _glDeleteVertexArrays(1, &array);
        //public static void DepthFunc(int func) => _glDepthFunc(func);
        public static void DepthMask(bool enabled) => _glDepthMask(enabled);
        public static void DepthRange(double near, double far) => _glDepthRange(near, far);
        public static void DetachShader(uint program, uint shader) => _glDetachShader(program, shader);
        //public static void Disable(int cap) => _glDisable(cap);
        public static void Disablei(int target, uint index) => _glDisablei(target, index);
        public static void DisableVertexAttribArray(uint index) => _glDisableVertexAttribArray(index);
        //public static void DrawArrays(int mode, int first, int count) => _glDrawArrays(mode, first, count);
        public static void DrawArraysInstanced(int mode, int first, int count, int instanceCount) => _glDrawArraysInstanced(mode, first, count, instanceCount);
        //public static void DrawBuffer(int buffer) => _glDrawBuffer(buffer);
        public static void DrawBuffers(int n, int* buffers) => _glDrawBuffers(n, buffers);
        public static void DrawElements(int mode, int count, int type, void* indices) => _glDrawElements(mode, count, type, indices);
        public static void DrawElementsBaseVertex(int mode, int count, int type, void* indices, int baseVertex) => _glDrawElementsBaseVertex(mode, count, type, indices, baseVertex);
        public static void DrawElementsInstanced(int mode, int count, int type, void* indices, int instanceCount) => _glDrawElementsInstanced(mode, count, type, indices, instanceCount);
        public static void DrawElementsInstancedBaseVertex(int mode, int count, int type, void* indices, int instanceCount, int baseVertex) => _glDrawElementsInstancedBaseVertex(mode, count, type, indices, instanceCount, baseVertex);
        //public static void Enable(int cap) => _glEnable(cap);
        public static void Enablei(int target, uint index) => _glEnablei(target, index);
        public static void EnableVertexAttribArray(uint index) => _glEnableVertexAttribArray(index);
        public static void EndConditionalRender() => _glEndConditionalRender();
        public static void EndQuery(int target) => _glEndQuery(target);
        public static void EndTransformFeedback() => _glEndTransformFeedback();
        public static void Finish() => _glFinish();
        public static void Flush() => _glFlush();
        public static void FlushMappedBufferRange(int target, int offset, int length) => _glFlushMappedBufferRange(target, new IntPtr(offset), new IntPtr(length));
        public static void FlushMappedBufferRange(int target, long offset, long length) => _glFlushMappedBufferRange(target, new IntPtr(offset), new IntPtr(length));
        public static void FramebufferRenderbuffer(int target, int attachment, int renderbufferTarget, uint renderbuffer) => _glFramebufferRenderbuffer(target, attachment, renderbufferTarget, renderbuffer);
        public static void FramebufferRenderbuffer(int attachment, uint renderbuffer) => _glFramebufferRenderbuffer(GL_FRAMEBUFFER, attachment, GL_RENDERBUFFER, renderbuffer);
        public static void FramebufferTexture(int target, int attachment, uint texture, int level) => _glFramebufferTexture(target, attachment, texture, level);
        public static void FramebufferTexture1D(int target, int attachment, int texTarget, uint texture, int level) => _glFramebufferTexture1D(target, attachment, texTarget, texture, level);
        public static void FramebufferTexture2D(int target, int attachment, int texTarget, uint texture, int level) => _glFramebufferTexture2D(target, attachment, texTarget, texture, level);
        public static void FramebufferTexture3D(int target, int attachment, int texTarget, uint texture, int level, int zOffset) => _glFramebufferTexture3D(target, attachment, texTarget, texture, level, zOffset);
        public static void FramebufferTextureLayer(int target, int attachment, uint texture, int level, int layer) => _glFramebufferTextureLayer(target, attachment, texture, level, layer);
        public static void FrontFace(int mode) => _glFrontFace(mode);
        public static void GenBuffers(int n, uint* buffers) => _glGenBuffers(n, buffers);
        public static void GenerateMipmap(int target) => _glGenerateMipmap(target);
        public static void GenFramebuffers(int n, uint* buffers) => _glGenFramebuffers(n, buffers);
        public static void GenQueries(int n, uint* ids) => _glGenQueries(n, ids);
        public static void GenRenderbuffers(int n, uint* buffers) => _glGenRenderbuffers(n, buffers);
        public static void GenSamplers(int count, uint* samplers) => _glGenSamplers(count, samplers);
        public static void GenTextures(int n, uint* textures) => _glGenTextures(n, textures);
        public static void GenVertexArrays(int n, uint* arrays) => _glGenVertexArrays(n, arrays);
        public static void GetActiveUniformBlockiv(uint program, uint uniformBlockIndex, int pname, int* args) => _glGetActiveUniformBlockiv(program, uniformBlockIndex, pname, args);
        public static void GetActiveUniformsiv(uint program, int uniformCount, uint* uniformIndices, int pname, int* args) => _glGetActiveUniformsiv(program, uniformCount, uniformIndices, pname, args);
        public static void GetAttachedShaders(uint program, int maxCount, int* count, uint* shaders) => _glGetAttachedShaders(program, maxCount, count, shaders);
        public static void GetBooleani_v(int target, uint index, bool* data) => _glGetBooleani_v(target, index, data);
        public static void GetBooleanv(int paramName, bool* data) => _glGetBooleanv(paramName, data);
        public static void GetBufferParameteri64v(int target, int pname, long* args) => _glGetBufferParameteri64v(target, pname, args);
        public static void GetBufferParameteriv(int target, int pname, int* args) => _glGetBufferParameteriv(target, pname, args);
        public static void GetBufferSubData(int target, int offset, int size, IntPtr data) => _glGetBufferSubData(target, new IntPtr(offset), new IntPtr(size), data.ToPointer());
        public static void GetBufferSubData(int target, long offset, long size, IntPtr data) => _glGetBufferSubData(target, new IntPtr(offset), new IntPtr(size), data.ToPointer());
        public static void GetBufferSubData(int target, int offset, int size, void* data) => _glGetBufferSubData(target, new IntPtr(offset), new IntPtr(size), data);
        public static void GetBufferSubData(int target, long offset, long size, void* data) => _glGetBufferSubData(target, new IntPtr(offset), new IntPtr(size), data);
        public static void GetCompressedTexImage(int target, int level, IntPtr pixels) => _glGetCompressedTexImage(target, level, pixels.ToPointer());
        public static void GetCompressedTexImage(int target, int level, void* pixels) => _glGetCompressedTexImage(target, level, pixels);
        public static void GetDoublev(int paramName, double* data) => _glGetDoublev(paramName, data);
        public static void GetFloatv(int paramName, float* data) => _glGetFloatv(paramName, data);
        public static void GetFramebufferAttachmentParameteriv(int target, int attachment, int pname, int* args) => _glGetFramebufferAttachmentParameteriv(target, attachment, pname, args);
        public static void GetInteger64i_v(int target, uint index, long* data) => _glGetInteger64i_v(target, index, data);
        public static void GetInteger64v(int paramName, long* data) => _glGetInteger64v(paramName, data);
        public static void GetIntegeri_v(int target, uint index, int* data) => _glGetIntegeri_v(target, index, data);
        public static void GetIntegerv(int paramName, int* data) => _glGetIntegerv(paramName, data);
        public static void GetMultisamplefv(int pname, uint index, float* val) => _glGetMultisamplefv(pname, index, val);
        public static void GetProgramiv(uint program, int pname, int* args) => _glGetProgramiv(program, pname, args);
        public static void GetQueryiv(int target, int pname, int* args) => _glGetQueryiv(target, pname, args);
        public static void GetQueryObjecti64v(uint id, int pname, long* args) => _glGetQueryObjecti64v(id, pname, args);
        public static void GetQueryObjectiv(uint id, int pname, int* args) => _glGetQueryObjectiv(id, pname, args);
        public static void GetQueryObjectui64v(uint id, int pname, ulong* args) => _glGetQueryObjectui64v(id, pname, args);
        public static void GetQueryObjectuiv(uint id, int pname, uint* args) => _glGetQueryObjectuiv(id, pname, args);
        public static void GetRenderbufferParameteriv(int target, int pname, int* args) => _glGetRenderbufferParameteriv(target, pname, args);
        public static void GetSamplerParameterfv(uint sampler, int paramName, float* args) => _glGetSamplerParameterfv(sampler, paramName, args);
        public static void GetSamplerParameterIiv(uint sampler, int paramName, int* args) => _glGetSamplerParameterIiv(sampler, paramName, args);
        public static void GetSamplerParameterIuiv(uint sampler, int paramName, uint* args) => _glGetSamplerParameterIuiv(sampler, paramName, args);
        public static void GetSamplerParameteriv(uint sampler, int paramName, int* args) => _glGetSamplerParameteriv(sampler, paramName, args);
        public static void GetShaderiv(uint shader, int pname, int* args) => _glGetShaderiv(shader, pname, args);
        public static void GetSynciv(IntPtr sync, int pname, int bufSize, int* length, int* values) => _glGetSynciv(sync, pname, bufSize, length, values);
        public static void GetTexImage(int target, int level, int format, int type, void* pixels) => _glGetTexImage(target, level, format, type, pixels);
        public static void GetTexImage(int target, int level, int format, int type, IntPtr pixels) => _glGetTexImage(target, level, format, type, pixels.ToPointer());
        public static void GetTexLevelParameterfv(int target, int level, int paramName, float* args) => _glGetTexLevelParameterfv(target, level, paramName, args);
        public static void GetTexLevelParameteriv(int target, int level, int paramName, int* args) => _glGetTexLevelParameteriv(target, level, paramName, args);
        public static void GetTexParameterfv(int target, int paramName, float* args) => _glGetTexParameterfv(target, paramName, args);
        public static void GetTexParameterIiv(int target, int pname, int* args) => _glGetTexParameterIiv(target, pname, args);
        public static void GetTexParameterIuiv(int target, int pname, uint* args) => _glGetTexParameterIuiv(target, pname, args);
        public static void GetTexParameteriv(int target, int paramName, int* args) => _glGetTexParameteriv(target, paramName, args);
        public static void GetUniformfv(uint program, int location, float* args) => _glGetUniformfv(program, location, args);
        public static void GetUniformiv(uint program, int location, int* args) => _glGetUniformiv(program, location, args);
        public static int GetUniformLocation(uint program, byte* name) => _glGetUniformLocation(program, name);
        public static void GetUniformuiv(uint program, int location, uint* args) => _glGetUniformuiv(program, location, args);
        public static void GetVertexAttribdv(uint index, int pname, double* args) => _glGetVertexAttribdv(index, pname, args);
        public static void GetVertexAttribfv(uint index, int pname, float* args) => _glGetVertexAttribfv(index, pname, args);
        public static void GetVertexAttribIiv(uint index, int pname, int* args) => _glGetVertexAttribIiv(index, pname, args);
        public static void GetVertexAttribIuiv(uint index, int pname, uint* args) => _glGetVertexAttribIuiv(index, pname, args);
        public static void GetVertexAttribiv(uint index, int pname, int* args) => _glGetVertexAttribiv(index, pname, args);
        public static void Hint(int target, int mode) => _glHint(target, mode);
        public static bool IsBuffer(uint buffer) => _glIsBuffer(buffer);
        public static bool IsEnabled(int cap) => _glIsEnabled(cap);
        public static bool IsEnabledi(int target, uint index) => _glIsEnabledi(target, index);
        public static bool IsFramebuffer(uint framebuffer) => _glIsFramebuffer(framebuffer);
        public static bool IsProgram(uint program) => _glIsProgram(program);
        public static bool IsQuery(uint id) => _glIsQuery(id);
        public static bool IsRenderbuffer(uint renderbuffer) => _glIsRenderbuffer(renderbuffer);
        public static bool IsSampler(uint sampler) => _glIsSampler(sampler);
        public static bool IsShader(uint shader) => _glIsShader(shader);
        public static bool IsSync(IntPtr sync) => _glIsSync(sync);
        public static bool IsTexture(uint texture) => _glIsTexture(texture);
        public static bool IsVertexArray(uint array) => _glIsVertexArray(array);
        public static void LineWidth(float width) => _glLineWidth(width);
        public static void LinkProgram(uint program) => _glLinkProgram(program);
        public static void LogicOp(int opcode) => _glLogicOp(opcode);
        public static void MultiDrawArrays(int mode, int* first, int* count, int drawCount) => _glMultiDrawArrays(mode, first, count, drawCount);
        public static void MultiTexCoordP1ui(int texture, int type, uint coords) => _glMultiTexCoordP1ui(texture, type, coords);
        public static void MultiTexCoordP1uiv(int texture, int type, uint* coords) => _glMultiTexCoordP1uiv(texture, type, coords);
        public static void MultiTexCoordP2ui(int texture, int type, uint coords) => _glMultiTexCoordP2ui(texture, type, coords);
        public static void MultiTexCoordP2uiv(int texture, int type, uint* coords) => _glMultiTexCoordP2uiv(texture, type, coords);
        public static void MultiTexCoordP3ui(int texture, int type, uint coords) => _glMultiTexCoordP3ui(texture, type, coords);
        public static void MultiTexCoordP3uiv(int texture, int type, uint* coords) => _glMultiTexCoordP3uiv(texture, type, coords);
        public static void MultiTexCoordP4ui(int texture, int type, uint coords) => _glMultiTexCoordP4ui(texture, type, coords);
        public static void MultiTexCoordP4uiv(int texture, int type, uint* coords) => _glMultiTexCoordP4uiv(texture, type, coords);
        public static void NormalP3ui(int type, uint coords) => _glNormalP3ui(type, coords);
        public static void NormalP3uiv(int type, uint* coords) => _glNormalP3uiv(type, coords);
        public static void PixelStoref(int paramName, float param) => _glPixelStoref(paramName, param);
        public static void PixelStorei(int paramName, int param) => _glPixelStorei(paramName, param);
        public static void PointParameterf(int paramName, float param) => _glPointParameterf(paramName, param);
        public static void PointParameterfv(int paramName, float* args) => _glPointParameterfv(paramName, args);
        public static void PointParameteri(int paramName, int param) => _glPointParameteri(paramName, param);
        public static void PointParameteriv(int paramName, int* args) => _glPointParameteriv(paramName, args);
        public static void PointSize(float size) => _glPointSize(size);
        public static void PolygonMode(int face, int mode) => _glPolygonMode(face, mode);
        public static void PolygonOffset(float factor, float units) => _glPolygonOffset(factor, units);
        public static void PrimitiveRestartIndex(uint index) => _glPrimitiveRestartIndex(index);
        public static void ProvokingVertex(int mode) => _glProvokingVertex(mode);
        public static void QueryCounter(uint id, int target) => _glQueryCounter(id, target);
        //public static void ReadBuffer(int buffer) => _glReadBuffer(buffer);
        public static void ReadPixels(int x, int y, int width, int height, int format, int type, void* pixels) => _glReadPixels(x, y, width, height, format, type, pixels);
        public static void ReadPixels(int x, int y, int width, int height, int format, int type, IntPtr pixels) => _glReadPixels(x, y, width, height, format, type, pixels.ToPointer());
        public static void RenderbufferStorage(int target, int internalFormat, int width, int height) => _glRenderbufferStorage(target, internalFormat, width, height);
        public static void RenderbufferStorageMultisample(int target, int samples, int internalformat, int width, int height) => _glRenderbufferStorageMultisample(target, samples, internalformat, width, height);
        public static void SampleCoverage(float value, bool invert) => _glSampleCoverage(value, invert);
        public static void SampleMaski(uint maskNumber, uint mask) => _glSampleMaski(maskNumber, mask);
        public static void SamplerParameterf(uint sampler, int paramName, float param) => _glSamplerParameterf(sampler, paramName, param);
        public static void SamplerParameterfv(uint sampler, int paramName, float* param) => _glSamplerParameterfv(sampler, paramName, param);
        public static void SamplerParameteri(uint sampler, int paramName, int param) => _glSamplerParameteri(sampler, paramName, param);
        public static void SamplerParameterIiv(uint sampler, int paramName, int* param) => _glSamplerParameterIiv(sampler, paramName, param);
        public static void SamplerParameterIuiv(uint sampler, int paramName, uint* param) => _glSamplerParameterIuiv(sampler, paramName, param);
        public static void SamplerParameteriv(uint sampler, int paramName, int* param) => _glSamplerParameteriv(sampler, paramName, param);
        public static void Scissor(int x, int y, int width, int height) => _glScissor(x, y, width, height);
        public static void SecondaryColorP3ui(int type, uint color) => _glSecondaryColorP3ui(type, color);
        public static void SecondaryColorP3uiv(int type, uint* color) => _glSecondaryColorP3uiv(type, color);
        public static void ShaderSource(uint shader, int count, byte** str, int* length) => _glShaderSource(shader, count, str, length);
        public static void StencilFunc(int func, int reference, uint mask) => _glStencilFunc(func, reference, mask);
        public static void StencilFuncSeparate(int face, int func, int reference, uint mask) => _glStencilFuncSeparate(face, func, reference, mask);
        public static void StencilMask(uint mask) => _glStencilMask(mask);
        public static void StencilMaskSeparate(int face, uint mask) => _glStencilMaskSeparate(face, mask);
        public static void StencilOp(int fail, int zfail, int zpass) => _glStencilOp(fail, zfail, zpass);
        public static void StencilOpSeparate(int face, int sfail, int dpfail, int dppass) => _glStencilOpSeparate(face, sfail, dpfail, dppass);
        public static void TexBuffer(int target, int internalFormat, uint buffer) => _glTexBuffer(target, internalFormat, buffer);
        public static void TexCoordP1ui(int type, uint coords) => _glTexCoordP1ui(type, coords);
        public static void TexCoordP1uiv(int type, uint* coords) => _glTexCoordP1uiv(type, coords);
        public static void TexCoordP2ui(int type, uint coords) => _glTexCoordP2ui(type, coords);
        public static void TexCoordP2uiv(int type, uint* coords) => _glTexCoordP2uiv(type, coords);
        public static void TexCoordP3ui(int type, uint coords) => _glTexCoordP3ui(type, coords);
        public static void TexCoordP3uiv(int type, uint* coords) => _glTexCoordP3uiv(type, coords);
        public static void TexCoordP4ui(int type, uint coords) => _glTexCoordP4ui(type, coords);
        public static void TexCoordP4uiv(int type, uint* coords) => _glTexCoordP4uiv(type, coords);
        public static void TexImage1D(int target, int level, int internalFormat, int width, int border, int format, int type, IntPtr pixels) => _glTexImage1D(target, level, internalFormat, width, border, format, type, pixels.ToPointer());
        public static void TexImage1D(int target, int level, int internalFormat, int width, int border, int format, int type, void* pixels) => _glTexImage1D(target, level, internalFormat, width, border, format, type, pixels);
        public static void TexImage2D(int target, int level, int internalFormat, int width, int height, int border, int format, int type, IntPtr pixels) => _glTexImage2D(target, level, internalFormat, width, height, border, format, type, pixels.ToPointer());
        public static void TexImage2D(int target, int level, int internalFormat, int width, int height, int border, int format, int type, void* pixels) => _glTexImage2D(target, level, internalFormat, width, height, border, format, type, pixels);
        public static void TexImage2DMultisample(int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations) => _glTexImage2DMultisample(target, samples, internalformat, width, height, fixedsamplelocations);
        public static void TexImage3D(int target, int level, int internalFormat, int width, int height, int depth, int border, int format, int type, IntPtr pixels) => _glTexImage3D(target, level, internalFormat, width, height, depth, border, format, type, pixels.ToPointer());
        public static void TexImage3D(int target, int level, int internalFormat, int width, int height, int depth, int border, int format, int type, void* pixels) => _glTexImage3D(target, level, internalFormat, width, height, depth, border, format, type, pixels);
        public static void TexImage3DMultisample(int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations) => _glTexImage3DMultisample(target, samples, internalformat, width, height, depth, fixedsamplelocations);
        public static void TexParameterf(int target, int paramName, float param) => _glTexParameterf(target, paramName, param);
        public static void TexParameterfv(int target, int paramName, float* param) => _glTexParameterfv(target, paramName, param);
        public static void TexParameteri(int target, int paramName, int param) => _glTexParameteri(target, paramName, param);
        public static void TexParameterIiv(int target, int pname, int* args) => _glTexParameterIiv(target, pname, args);
        public static void TexParameterIuiv(int target, int pname, uint* args) => _glTexParameterIuiv(target, pname, args);
        public static void TexParameteriv(int target, int paramName, int* param) => _glTexParameteriv(target, paramName, param);
        public static void TexSubImage1D(int target, int level, int xOffset, int width, int format, int type, IntPtr pixels) => _glTexSubImage1D(target, level, xOffset, width, format, type, pixels.ToPointer());
        public static void TexSubImage1D(int target, int level, int xOffset, int width, int format, int type, void* pixels) => _glTexSubImage1D(target, level, xOffset, width, format, type, pixels);
        public static void TexSubImage2D(int target, int level, int xOffset, int yOffset, int width, int height, int format, int type, IntPtr pixels) => _glTexSubImage2D(target, level, xOffset, yOffset, width, height, format, type, pixels.ToPointer());
        public static void TexSubImage2D(int target, int level, int xOffset, int yOffset, int width, int height, int format, int type, void* pixels) => _glTexSubImage2D(target, level, xOffset, yOffset, width, height, format, type, pixels);
        public static void TexSubImage3D(int target, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, void* pixels) => _glTexSubImage3D(target, level, xOffset, yOffset, zOffset, width, height, depth, format, type, pixels);
        public static void TransformFeedbackVaryings(uint program, int count, byte** varyings, int bufferMode) => _glTransformFeedbackVaryings(program, count, varyings, bufferMode);
        public static void Uniform1f(int location, float v0) => _glUniform1f(location, v0);
        public static void Uniform1fv(int location, int count, float* value) => _glUniform1fv(location, count, value);
        public static void Uniform1i(int location, int v0) => _glUniform1i(location, v0);
        public static void Uniform1iv(int location, int count, int* value) => _glUniform1iv(location, count, value);
        public static void Uniform1ui(int location, uint v0) => _glUniform1ui(location, v0);
        public static void Uniform1uiv(int location, int count, uint* value) => _glUniform1uiv(location, count, value);
        public static void Uniform2f(int location, float v0, float v1) => _glUniform2f(location, v0, v1);
        public static void Uniform2fv(int location, int count, float* value) => _glUniform2fv(location, count, value);
        public static void Uniform2i(int location, int v0, int v1) => _glUniform2i(location, v0, v1);
        public static void Uniform2iv(int location, int count, int* value) => _glUniform2iv(location, count, value);
        public static void Uniform2ui(int location, uint v0, uint v1) => _glUniform2ui(location, v0, v1);
        public static void Uniform2uiv(int location, int count, uint* value) => _glUniform2uiv(location, count, value);
        public static void Uniform3f(int location, float v0, float v1, float v2) => _glUniform3f(location, v0, v1, v2);
        public static void Uniform3fv(int location, int count, float* value) => _glUniform3fv(location, count, value);
        public static void Uniform3i(int location, int v0, int v1, int v2) => _glUniform3i(location, v0, v1, v2);
        public static void Uniform3iv(int location, int count, int* value) => _glUniform3iv(location, count, value);
        public static void Uniform3ui(int location, uint v0, uint v1, uint v2) => _glUniform3ui(location, v0, v1, v2);
        public static void Uniform3uiv(int location, int count, uint* value) => _glUniform3uiv(location, count, value);
        public static void Uniform4f(int location, float v0, float v1, float v2, float v3) => _glUniform4f(location, v0, v1, v2, v3);
        public static void Uniform4fv(int location, int count, float* value) => _glUniform4fv(location, count, value);
        public static void Uniform4i(int location, int v0, int v1, int v2, int v3) => _glUniform4i(location, v0, v1, v2, v3);
        public static void Uniform4iv(int location, int count, int* value) => _glUniform4iv(location, count, value);
        public static void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3) => _glUniform4ui(location, v0, v1, v2, v3);
        public static void Uniform4uiv(int location, int count, uint* value) => _glUniform4uiv(location, count, value);
        public static void UniformBlockBinding(uint program, uint uniformBlockIndex, uint uniformBlockBinding) => _glUniformBlockBinding(program, uniformBlockIndex, uniformBlockBinding);
        public static void UniformMatrix2fv(int location, int count, bool transpose, float* value) => _glUniformMatrix2fv(location, count, transpose, value);
        public static void UniformMatrix2x3fv(int location, int count, bool transpose, float* value) => _glUniformMatrix2x3fv(location, count, transpose, value);
        public static void UniformMatrix2x4fv(int location, int count, bool transpose, float* value) => _glUniformMatrix2x4fv(location, count, transpose, value);
        public static void UniformMatrix3fv(int location, int count, bool transpose, float* value) => _glUniformMatrix3fv(location, count, transpose, value);
        public static void UniformMatrix3x2fv(int location, int count, bool transpose, float* value) => _glUniformMatrix3x2fv(location, count, transpose, value);
        public static void UniformMatrix3x4fv(int location, int count, bool transpose, float* value) => _glUniformMatrix3x4fv(location, count, transpose, value);
        public static void UniformMatrix4fv(int location, int count, bool transpose, float* value) => _glUniformMatrix4fv(location, count, transpose, value);
        public static void UniformMatrix4x2fv(int location, int count, bool transpose, float* value) => _glUniformMatrix4x2fv(location, count, transpose, value);
        public static void UniformMatrix4x3fv(int location, int count, bool transpose, float* value) => _glUniformMatrix4x3fv(location, count, transpose, value);
        public static bool UnmapBuffer(int target) => _glUnmapBuffer(target);
        public static void UseProgram(uint program) => _glUseProgram(program);
        public static void ValidateProgram(uint program) => _glValidateProgram(program);
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
        public static void VertexAttrib4bv(uint index, sbyte* v) => _glVertexAttrib4bv(index, v);
        public static void VertexAttrib4d(uint index, double x, double y, double z, double w) => _glVertexAttrib4d(index, x, y, z, w);
        public static void VertexAttrib4dv(uint index, double* v) => _glVertexAttrib4dv(index, v);
        public static void VertexAttrib4f(uint index, float x, float y, float z, float w) => _glVertexAttrib4f(index, x, y, z, w);
        public static void VertexAttrib4fv(uint index, float* v) => _glVertexAttrib4fv(index, v);
        public static void VertexAttrib4iv(uint index, int* v) => _glVertexAttrib4iv(index, v);
        public static void VertexAttrib4Nbv(uint index, sbyte* v) => _glVertexAttrib4Nbv(index, v);
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
        public static void VertexAttribDivisor(uint index, uint divisor) => _glVertexAttribDivisor(index, divisor);
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
        public static void VertexAttribI4bv(uint index, sbyte* v) => _glVertexAttribI4bv(index, v);
        public static void VertexAttribI4i(uint index, int x, int y, int z, int w) => _glVertexAttribI4i(index, x, y, z, w);
        public static void VertexAttribI4iv(uint index, int* v) => _glVertexAttribI4iv(index, v);
        public static void VertexAttribI4sv(uint index, short* v) => _glVertexAttribI4sv(index, v);
        public static void VertexAttribI4ubv(uint index, byte* v) => _glVertexAttribI4ubv(index, v);
        public static void VertexAttribI4ui(uint index, uint x, uint y, uint z, uint w) => _glVertexAttribI4ui(index, x, y, z, w);
        public static void VertexAttribI4uiv(uint index, uint* v) => _glVertexAttribI4uiv(index, v);
        public static void VertexAttribI4usv(uint index, ushort* v) => _glVertexAttribI4usv(index, v);
        public static void VertexAttribIPointer(uint index, int size, int type, int stride, void* pointer) => _glVertexAttribIPointer(index, size, type, stride, pointer);
        public static void VertexAttribIPointer(uint index, int size, int type, int stride, IntPtr pointer) => _glVertexAttribIPointer(index, size, type, stride, pointer.ToPointer());
        public static void VertexAttribP1ui(uint index, int type, bool normalized, uint value) => _glVertexAttribP1ui(index, type, normalized, value);
        public static void VertexAttribP1uiv(uint index, int type, bool normalized, uint* value) => _glVertexAttribP1uiv(index, type, normalized, value);
        public static void VertexAttribP2ui(uint index, int type, bool normalized, uint value) => _glVertexAttribP2ui(index, type, normalized, value);
        public static void VertexAttribP2uiv(uint index, int type, bool normalized, uint* value) => _glVertexAttribP2uiv(index, type, normalized, value);
        public static void VertexAttribP3ui(uint index, int type, bool normalized, uint value) => _glVertexAttribP3ui(index, type, normalized, value);
        public static void VertexAttribP3uiv(uint index, int type, bool normalized, uint* value) => _glVertexAttribP3uiv(index, type, normalized, value);
        public static void VertexAttribP4ui(uint index, int type, bool normalized, uint value) => _glVertexAttribP4ui(index, type, normalized, value);
        public static void VertexAttribP4uiv(uint index, int type, bool normalized, uint* value) => _glVertexAttribP4uiv(index, type, normalized, value);
        public static void VertexAttribPointer(uint index, int size, int type, bool normalized, int stride, IntPtr pointer) => _glVertexAttribPointer(index, size, type, normalized, stride, pointer.ToPointer());
        public static void VertexP2ui(int type, uint value) => _glVertexP2ui(type, value);
        public static void VertexP2uiv(int type, uint* value) => _glVertexP2uiv(type, value);
        public static void VertexP3ui(int type, uint value) => _glVertexP3ui(type, value);
        public static void VertexP3uiv(int type, uint* value) => _glVertexP3uiv(type, value);
        public static void VertexP4ui(int type, uint value) => _glVertexP4ui(type, value);
        public static void VertexP4uiv(int type, uint* value) => _glVertexP4uiv(type, value);
        public static void Viewport(int x, int y, int width, int height) => _glViewport(x, y, width, height);
        public static void WaitSync(IntPtr sync, uint flags, ulong timeout) => _glWaitSync(sync, flags, timeout);
        
        private static T GetFunctionDelegate<T>(string name) where T : Delegate
        {
            var address = GLFW.GetProcAddress(name);
            return Marshal.GetDelegateForFunctionPointer<T>(address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool DEL_B_I(int v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool DEL_B_IUi(int v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool DEL_B_P(IntPtr v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool DEL_B_Ui(uint v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* DEL_Byp_I(int v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* DEL_Byp_IUi(int v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int DEL_I_();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int DEL_I_I(int v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int DEL_I_PUiUl(IntPtr v0, uint v1, ulong v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int DEL_I_UiByp(uint v0, byte* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr DEL_P_IUi(int v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint DEL_Ui_();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint DEL_Ui_I(int v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint DEL_Ui_UiByp(uint v0, byte* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_B(bool v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_BBBB(bool v0, bool v1, bool v2, bool v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_D(double v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_DD(double v0, double v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_F(float v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_FB(float v0, bool v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_FF(float v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_FFFF(float v0, float v1, float v2, float v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_I(int v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IBp(int v0, bool* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IDp(int v0, double* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IF(int v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IFF(int v0, float v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IFFF(int v0, float v1, float v2, float v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IFFFF(int v0, float v1, float v2, float v3, float v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IFp(int v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_II(int v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIBFp(int v0, int v1, bool v2, float* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIF(int v0, int v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIFI(int v0, int v1, float v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIFp(int v0, int v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_III(int v0, int v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIFp(int v0, int v1, int v2, float* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIII(int v0, int v1, int v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIII(int v0, int v1, int v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIB(int v0, int v1, int v2, int v3, int v4, bool v5);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIII(int v0, int v1, int v2, int v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIIB(int v0, int v1, int v2, int v3, int v4, int v5, bool v6);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIII(int v0, int v1, int v2, int v3, int v4, int v5, int v6);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIIII(int v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIIIII(int v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIIIIIIVp(int v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, int v9, void* v10);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIIIIIVp(int v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, int v8, void* v9);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIIIIUiI(int v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, uint v8, int v9);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIIIIVp(int v0, int v1, int v2, int v3, int v4, int v5, int v6, int v7, void* v8);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIIIVp(int v0, int v1, int v2, int v3, int v4, int v5, int v6, void* v7);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIIIVp(int v0, int v1, int v2, int v3, int v4, int v5, void* v6);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIp(int v0, int v1, int v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIIVp(int v0, int v1, int v2, int v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIp(int v0, int v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIUi(int v0, int v1, int v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIUiI(int v0, int v1, int v2, uint v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIUiII(int v0, int v1, int v2, uint v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIVp(int v0, int v1, int v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIVpI(int v0, int v1, int v2, void* v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIIVpII(int v0, int v1, int v2, void* v3, int v4, int v5);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IILp(int v0, int v1, long* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIOp(int v0, int v1, out IntPtr v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIp(int v0, int* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIpIpI(int v0, int* v1, int* v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIpIVppI(int v0, int* v1, int v2, void** v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIpIVppIIp(int v0, int* v1, int v2, void** v3, int v4, int* v5);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIPPP(int v0, int v1, IntPtr v2, IntPtr v3, IntPtr v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIUi(int v0, int v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIUiI(int v0, int v1, uint v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIUiII(int v0, int v1, uint v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIUip(int v0, int v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IIVp(int v0, int v1, void* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_ILp(int v0, long* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IPP(int v0, IntPtr v1, IntPtr v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IPPVp(int v0, IntPtr v1, IntPtr v2, void* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IPVpI(int v0, IntPtr v1, void* v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUi(int v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiBp(int v0, uint v1, bool* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiFp(int v0, uint v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiIp(int v0, uint v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiLp(int v0, uint v1, long* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUip(int v0, uint* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiUi(int v0, uint v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiUiIIVp(int v0, uint v1, uint v2, int v3, int v4, void* v5);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiUiIIVpI(int v0, uint v1, uint v2, int v3, int v4, void* v5, int v6);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiUiPP(int v0, uint v1, uint v2, IntPtr v3, IntPtr v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiUiUi(int v0, uint v1, uint v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_IUiUiUiUi(int v0, uint v1, uint v2, uint v3, uint v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_P(IntPtr v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_PIIIpIp(IntPtr v0, int v1, int v2, int* v3, int* v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_PUiUl(IntPtr v0, uint v1, ulong v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_Ui(uint v0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiBBBB(uint v0, bool v1, bool v2, bool v3, bool v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiByByByBy(uint v0, byte v1, byte v2, byte v3, byte v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiByp(uint v0, byte* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiD(uint v0, double v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiDD(uint v0, double v1, double v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiDDD(uint v0, double v1, double v2, double v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiDDDD(uint v0, double v1, double v2, double v3, double v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiDp(uint v0, double* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiF(uint v0, float v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiFF(uint v0, float v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiFFF(uint v0, float v1, float v2, float v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiFFFF(uint v0, float v1, float v2, float v3, float v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiFp(uint v0, float* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiI(uint v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIBUi(uint v0, int v1, bool v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIBUip(uint v0, int v1, bool v2, uint* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIByppI(uint v0, int v1, byte** v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIByppIp(uint v0, int v1, byte** v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIByppUip(uint v0, int v1, byte** v2, uint* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIDp(uint v0, int v1, double* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIF(uint v0, int v1, float v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIFp(uint v0, int v1, float* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiII(uint v0, int v1, int v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIIBIVp(uint v0, int v1, int v2, bool v3, int v4, void* v5);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIII(uint v0, int v1, int v2, int v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIIII(uint v0, int v1, int v2, int v3, int v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIIIVp(uint v0, int v1, int v2, int v3, void* v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIIp(uint v0, int v1, int* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIIpByp(uint v0, int v1, int* v2, byte* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIIpUip(uint v0, int v1, int* v2, uint* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiILp(uint v0, int v1, long* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIOp(uint v0, int v1, out IntPtr v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIp(uint v0, int* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIUip(uint v0, int v1, uint* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIUipIIp(uint v0, int v1, uint* v2, int v3, int* v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiIUlp(uint v0, int v1, ulong* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiS(uint v0, short v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiSbyp(uint v0, sbyte* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiSp(uint v0, short* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiSS(uint v0, short v1, short v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiSSS(uint v0, short v1, short v2, short v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiSSSS(uint v0, short v1, short v2, short v3, short v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUi(uint v0, uint v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUiByp(uint v0, uint v1, byte* v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUiIIp(uint v0, uint v1, int v2, int* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUiIIpByp(uint v0, uint v1, int v2, int* v3, byte* v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUiIOiOiOiP(uint v0, uint v1, int v2, out int v3, out int v4, out int v5, IntPtr v6);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUip(uint v0, uint* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUiUi(uint v0, uint v1, uint v2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUiUiByp(uint v0, uint v1, uint v2, byte* v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUiUiUi(uint v0, uint v1, uint v2, uint v3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUiUiUiUi(uint v0, uint v1, uint v2, uint v3, uint v4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void DEL_V_UiUsp(uint v0, ushort* v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void* DEL_Vp_II(int v0, int v1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void* DEL_Vp_IPPUi(int v0, IntPtr v1, IntPtr v2, uint v3);

    }
}

