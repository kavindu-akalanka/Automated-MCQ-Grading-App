import cv2
import numpy as np
import utils
import os

path = utils.getPath() + 'Answer Sheet Images'
sourceImages = []
imageNames = []
dirContents = os.listdir(path)
for dc in dirContents:
    currentImage = cv2.imread(f'{path}/{dc}')
    sourceImages.append(currentImage)
    imageNames.append(os.path.splitext(dc)[0])
# print(imageNames)

questions = int(utils.getR())
choices = int(utils.getC())
ans = utils.getAnswerList()
w = 50 * choices
h = 50 * questions

for pathImg, regNo in zip(dirContents, imageNames):
    pathImg = path + '\\' + pathImg
    print(regNo)
    # reading and pre-processing image
    img = cv2.imread(pathImg)
    imgContour = img.copy()
    imgBiggestCont = img.copy()
    imgGray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    imgBlur = cv2.GaussianBlur(imgGray, (5, 5), 1)
    imgCanny = cv2.Canny(imgBlur, 10, 50)

    # find contours and draw them
    contours, hierarchy = cv2.findContours(imgCanny, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
    cv2.drawContours(imgContour, contours, -1, (207, 79, 0), 2)

    # find rectangles
    rectCon = utils.rectContour(contours)
    biggestContour = utils.getCornerPoints(rectCon[0])
    # print(biggestContour)

    if biggestContour.size != 0:
        cv2.drawContours(imgBiggestCont, biggestContour, -1, (0, 0, 255), 10)
        biggestContour = utils.re_order(biggestContour)

        pt1 = np.float32(biggestContour)
        pt2 = np.float32([[0, 0], [w, 0], [0, h], [w, h]])
        matrix = cv2.getPerspectiveTransform(pt1, pt2)
        imgWarpColored = cv2.warpPerspective(img, matrix, (w, h))

        # convert to threshold
        imgWarpGray = cv2.cvtColor(imgWarpColored, cv2.COLOR_BGR2GRAY)
        imgThreshold = cv2.threshold(imgWarpGray, 150, 255, cv2.THRESH_BINARY_INV)[1]
        boxes = utils.splitBoxes(imgThreshold)
        # cv2.imshow("2", boxes[2])
        # cv2.imshow("3", boxes[3])
        # print(cv2.countNonZero(boxes[2]), cv2.countNonZero(boxes[3]))

        # getting non-zero pixel values of each boxes (white pixels)
        pixelValue = np.zeros((questions, choices))
        countC = 0
        countR = 0
        for image in boxes:
            totalPixels = cv2.countNonZero(image)
            pixelValue[countR][countC] = totalPixels
            countC += 1
            if countC == choices:
                countR += 1
                countC = 0

        # finding index values of the markings
        maxIndexes = []
        for x in range (0, questions):
            arr = pixelValue[x]
            maxIndexVal = np.where(arr == np.amax(arr))
            # print (maxIndexVal[0])
            maxIndexes.append(maxIndexVal[0][0])

        # Grading
        correctAnsCount = 0
        for x in range (0, questions):
            if ans[x] == maxIndexes[x]:
                correctAnsCount += 1

        print(correctAnsCount)
        utils.write2csv(regNo, correctAnsCount)

        # cv2.imshow('Image', imgThreshold)
        # cv2.waitKey(0)
