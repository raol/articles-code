from __future__ import print_function
import numpy as np
from sklearn import datasets, linear_model

data = np.loadtxt('ex1data1.txt', delimiter=',')
X = data[:, 0:1]
Y = data[:, 1:2]

regr = linear_model.LinearRegression()
regr.fit(X, Y)

print('Coefficients: Theta_0: {0}, Theta_1: {1}'.format(regr.intercept_[0],  regr.coef_[0][0]))
