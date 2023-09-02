import cv2
import numpy as np
import os

def rectContour(contours):
    rectangleContours = []
    for i in contours:
        area = cv2.contourArea(i)
        # print(area)
        if area > 50:
            perimeter = cv2.arcLength(i, True)
            approx = cv2.approxPolyDP(i, 0.02*perimeter, True)
            # print("Corner points : ", len(approx))
            if len(approx) == 4:
                rectangleContours.append(i)

    # print("Rectangle", rectangleContours)
    rectangleContours = sorted(rectangleContours, key=cv2.contourArea, reverse=True)
    return rectangleContours

def getCornerPoints(cont):
    perimeter = cv2.arcLength(cont, True)
    approx = cv2.approxPolyDP(cont, 0.02 * perimeter, True)
    return approx

def re_order(myPoints):
    myPoints = myPoints.reshape((4, 2))
    mypointsNew = np.zeros((4, 1, 2), np.int32)
    add = myPoints.sum(1)
    # print(myPoints)
    # print(add)
    mypointsNew[0] = myPoints[np.argmin(add)]  # [0, 0]
    mypointsNew[3] = myPoints[np.argmax(add)]  # [w, h]
    diff = np.diff(myPoints, axis=1)
    mypointsNew[1] = myPoints[np.argmin(diff)]  # [w, 0]
    mypointsNew[2] = myPoints[np.argmax(diff)]  # [0, h]
    # print(diff)
    return mypointsNew

def splitBoxes(img):
    rows = np.vsplit(img, int(getR()))  # 10 = number of rows
    boxes = []
    for r in rows:
        cols = np.hsplit(r,int(getC()))  # 5 = number of columns
        for box in cols:
            boxes.append(box)
            # cv2.imshow("Split", box)
    return boxes

def getPath():
    workingDir = os.getcwd()
    workingDir = format(workingDir)
    workingDir = workingDir + '\\'

    return workingDir

def getR():
    rcCountPath = getPath() + "rcCount.txt"
    with open(rcCountPath, 'r+') as f:
        rc = f.readlines()[0]
        return rc.split(',')[0]

def getC():
    rcCountPath = getPath() + "rcCount.txt"
    with open(rcCountPath, 'r+') as f:
        rc = f.readlines()[0]
        return rc.split(',')[1]

def getAnswerList():
    ansIndexPath = getPath() + "ansIndex.txt"
    ans = []
    with open(ansIndexPath, 'r+') as f:
        ansTempList = f.readlines()
        for x in ansTempList:
            ans.append(int(x))
        return ans

def write2csv(regNo, mark):
    gradesCSVpath = getPath() + 'Results Sheet\\Grades.csv'
    with open(gradesCSVpath, 'r+') as f:
        data_list = f.readlines()
        name_list = []
        for line in data_list:
            record = line.split(',')
            name_list.append(record[0])
        if regNo not in name_list:
            f.writelines(f'\n{regNo}, {mark}')

