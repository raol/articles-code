import matplotlib.pyplot as plt
import numpy as np

def costFunction(theta, x, y):
    return (sum((x.dot(theta) - y) ** 2) / (2 * x.size))[0]

def gradientDescent(theta, x, y):
    m = y.size;
    numiter = 1500
    alpha = 0.01
    costHistory = []
    for i in range(0, numiter):
        theta = theta - (alpha / m) * np.transpose(x).dot(x.dot(theta) - y)
        costHistory.append(costFunction(theta, x, y))
    return costHistory, theta

data = np.loadtxt('ex1data1.txt', delimiter=',')

X = data[:, 0]
Y = data[:, 1]
Y = Y.reshape((Y.size, 1))
m = X.size

t = np.ones(shape=(m, 2))
t[:, 1] = X
X = t;
theta = np.array([0, 0])
theta = theta.reshape((2, 1))

costHistory, theta = gradientDescent(theta, X, Y)


plt.xlabel("Iterations count")
plt.ylabel("Theta value")
plt.axis([0, 1500, min(costHistory), max(costHistory)])

plt.plot(range(0, 1500), costHistory)

plt.show()