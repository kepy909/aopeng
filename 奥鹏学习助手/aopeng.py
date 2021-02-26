import random
 
a= []
b=[]
c=[]
 
max = 10 ** 8
nums =0
numsA=0
for i in range(10000):
    x = random.randint(0, max)
    while x in a:
        x = random.randint(0, max)
    nums+=x
    a.append(x)
a.sort()
for index in a:
    if numsA+index<nums/2 :
        numsA+=index
        b.append(index)
    else:
        c.append(index)
mins =(nums/2)-numsA
print("差值："+str((nums/2)-numsA))
for index in range(len(b)):
    for indexc in range(len(c)):
        if (b[index]+mins) == c[indexc]:
            print("存在值"+str(mins)+",C数值"+str(c[indexc])+"在数组b里面："+str(b[index]))
            numsA +=mins
            mins=99999999999999999
            numsss=b[index]
            b[index]=c[indexc]
            c[indexc]=numsss
numb=0
numc=0
for p in b:
    numb+=p
for o in c:
    numc+=o
 
print(nums/2)
print("numb"+str(numb))
print("numc"+str(numc))