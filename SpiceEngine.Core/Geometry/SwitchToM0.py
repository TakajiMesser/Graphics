import fileinput
import os

class Replacement(object):
    old = ""
    new = ""

    def __init__(self, old, new):
        self.old = old
        self.new = new

def getReplacements():
    replacements = []
    replacements.append(Replacement("M11", "M00"))
    replacements.append(Replacement("M12", "M01"))
    replacements.append(Replacement("M13", "M02"))
    replacements.append(Replacement("M14", "M03"))

    replacements.append(Replacement("M21", "M10"))
    replacements.append(Replacement("M22", "M11"))
    replacements.append(Replacement("M23", "M12"))
    replacements.append(Replacement("M24", "M13"))

    replacements.append(Replacement("M31", "M20"))
    replacements.append(Replacement("M32", "M21"))
    replacements.append(Replacement("M33", "M22"))
    replacements.append(Replacement("M34", "M23"))

    replacements.append(Replacement("M41", "M30"))
    replacements.append(Replacement("M42", "M31"))
    replacements.append(Replacement("M43", "M32"))
    replacements.append(Replacement("M44", "M33"))

    return replacements

def performReplacements(relativePath, replacements):
    currentDirectory = os.path.dirname(__file__)
    absolutePath = os.path.join(currentDirectory, relativePath)

    with open(absolutePath, 'r') as inputFile:
        fileData = inputFile.read()

    for replacement in replacements:
        fileData = fileData.replace(replacement.old, replacement.new)

    with open(absolutePath, 'w') as outputFile:
        outputFile.write(fileData)

relativePath = os.path.join("Custom", "Matrix4.cs")
replacements = getReplacements()
performReplacements(relativePath, replacements)
