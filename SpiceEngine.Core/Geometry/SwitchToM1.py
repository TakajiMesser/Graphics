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
    replacements.append(Replacement("M30", "M41"))  
    replacements.append(Replacement("M31", "M42"))
    replacements.append(Replacement("M32", "M43"))
    replacements.append(Replacement("M33", "M44"))

    replacements.append(Replacement("M20", "M31"))
    replacements.append(Replacement("M21", "M32"))
    replacements.append(Replacement("M22", "M33"))
    replacements.append(Replacement("M23", "M34"))

    replacements.append(Replacement("M10", "M21"))
    replacements.append(Replacement("M11", "M22"))
    replacements.append(Replacement("M12", "M23"))
    replacements.append(Replacement("M13", "M24"))

    replacements.append(Replacement("M00", "M11"))
    replacements.append(Replacement("M01", "M12"))
    replacements.append(Replacement("M02", "M13"))
    replacements.append(Replacement("M03", "M14"))

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
