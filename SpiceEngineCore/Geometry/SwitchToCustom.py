import fileinput
import os

class Replacement(object):
    old = ""
    new = ""

    def __init__(self, old, new):
        self.old = old
        self.new = new

def getRelativePaths(directory):
    relativePaths = []
    fileNames = ["Color4.cs", "Matrix2.cs", "Matrix3.cs", "Matrix4.cs", "Quaternion.cs", "Vector2.cs", "Vector3.cs", "Vector4.cs"]
    
    for fileName in fileNames:
        relativePath = os.path.join(directory, fileName)
        relativePaths.append(relativePath)

    return relativePaths

def getEnablingReplacements():
    replacements = []
    replacements.append(Replacement("CColor4", "Color4"))
    #replacements.append(Replacement("CMatrix2", "Matrix2"))
    #replacements.append(Replacement("CMatrix3", "Matrix3"))
    #replacements.append(Replacement("CMatrix4", "Matrix4"))
    #replacements.append(Replacement("CQuaternion", "Quaternion"))
    replacements.append(Replacement("CVector2", "Vector2"))
    replacements.append(Replacement("CVector3", "Vector3"))
    replacements.append(Replacement("CVector4", "Vector4"))

    return replacements

def getDisablingReplacements():
    replacements = []
    replacements.append(Replacement("Color4", "TColor4"))
    #replacements.append(Replacement("Matrix2", "TMatrix2"))
    #replacements.append(Replacement("Matrix3", "TMatrix3"))
    #replacements.append(Replacement("Matrix4", "TMatrix4"))
    #replacements.append(Replacement("Quaternion", "TQuaternion"))
    replacements.append(Replacement("Vector2", "TVector2"))
    replacements.append(Replacement("Vector3", "TVector3"))
    replacements.append(Replacement("Vector4", "TVector4"))

    return replacements

def performReplacements(relativePath, replacements):
    currentDirectory = os.path.dirname(__file__)
    absolutePath = os.path.join(currentDirectory, relativePath)

    with open(absolutePath, 'r') as inputFile:
        fileData = inputFile.read()

    for replacement in replacements:
        print("For " + absolutePath + ", replace " + replacement.old + " with " + replacement.new)
        fileData = fileData.replace(replacement.old, replacement.new)

    with open(absolutePath, 'w') as outputFile:
        outputFile.write(fileData)

enablingPaths = getRelativePaths("Custom")
enablingReplacements = getEnablingReplacements()

for path in enablingPaths:
    performReplacements(path, enablingReplacements)

disablingPaths = getRelativePaths("TK")
disablingReplacements = getDisablingReplacements()

for path in disablingPaths:
    performReplacements(path, disablingReplacements)
