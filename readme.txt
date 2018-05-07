This code crawls for all course zip materials from a particular subject.

In this example, I am using the html file from Mathematics. You need to first save the web page to some local location.
The code will extract all the module codes and download the zip files accordingly with some structured url pattern.

You need to change 3 things when you change the subject html page.
1) htmlPath = @"C:\someFolder\Mathematics   MIT OpenCourseWare   Free Online Course Materials";
Change the above path accordingly to the html

2)var splitter = @"https://ocw.mit.edu/courses/mathematics/";
The code requires a split string to identify course codes. See below, the split string is the string after href="..." and before "18-303-linear-partial...".

<a rel="coursePreview" class="preview" href="https://ocw.mit.edu/courses/mathematics/18-303-linear-partial-differential-equations-analysis-and-numerics-fall-2014">18.303</a>

3) var modID = "18"
See the example above. Module ID follows immediately after the splitter. 

The splitter and modID is designed as such because some html pages have mixed module codes and subject names, making it messy(you may download engineering modules while crawling the Mathematics web page)  if you were to do a raw extraction. 