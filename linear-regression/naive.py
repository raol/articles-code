import numpy as np
import matplotlib.pyplot as plt


def costFunction(theta, x, y):
    cost = 0
    for i in zip(x, y):
        cost = cost + ((h(theta, i[0]) - i[1]) ** 2);
    return cost / (2 * len(x))

def h(theta, x):
    return theta[0] + theta[1] * x

def gradientDescent(x, y):
    # initialize alpha value
    alpha = 0.01
    iternum = 1500
    # set up initial theta values
    theta = [0, 0]
    for i in range(0, iternum):
        theta_0 = 0
        theta_1 = 0
        for i in zip(x, y):
            theta_0 += (h(theta, i[0]) - i[1])
            theta_1 += (h(theta, i[0]) - i[1]) * i[0]

        theta[0] = theta[0] - alpha*theta_0 / len(x)
        theta[1] = theta[1] - alpha*theta_1 / len(x)

    return theta;

f = open('ex1data1.txt')
a = zip(*[[float(l.split(',')[0]), float(l.split(',')[1])] for l in f.readlines()])

x = a[0]
y = a[1];
minX = min(*x)
maxX = max(*x)
minY = min(*y)
maxY = max(*y)

plt.xlabel("Population of City in 10,000s")
plt.ylabel("Profit in $10K")
plt.axis([min(*x) - 1, max(*x) + 1, min(*y) - 1, max(*y) + 1])

plt.plot(x, y, 'ro', label='training data')

tt = gradientDescent(x, y)
print(tt)
xn = np.arange(minX, maxX, 1)
plt.plot(xn, tt[0] + tt[1]*xn, label='function')


plt.legend(loc=2)
plt.show()

print(tt[0] + 50*tt[1])