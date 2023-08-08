from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.ui import Select
import time
import json
import sys

def set_option_field(name, value):
    elem = Select(driver.find_element(By.NAME, name))
    elem.select_by_value(value)
    return elem

def set_text_field(name, value):
    elem = driver.find_element(By.NAME, name)
    elem.click()
    elem.send_keys(Keys.HOME)
    elem.send_keys(value)
    return elem

f = open('schedule.json')
data = json.load(f)

args_len = (len(sys.argv))
project_id = sys.argv[1]

driver = webdriver.Chrome()
driver.get("https://luby-timesheet.azurewebsites.net/Worksheet/Read")

wait = WebDriverWait(driver, 120)

if(args_len == 4):
    set_text_field("Login",sys.argv[2])
    set_text_field("Password", sys.argv[3])
    driver.find_element(By.XPATH, "//button").click()

element = wait.until(EC.title_contains('Sistema'))
driver.get("https://luby-timesheet.azurewebsites.net/Worksheet/Read")

for row in data:

    wait.until(EC.presence_of_element_located((By.ID, "IdCustomer")))
    set_option_field("IdCustomer", "8213")

    wait.until(EC.presence_of_element_located((By.XPATH, "//option[@value='18542']")))
    set_option_field("IdProject", project_id)

    wait.until(EC.presence_of_element_located((By.XPATH, "//option[@value='1']")))
    set_option_field("IdCategory", str(row['Category']))

    set_text_field("InformedDate",row['Date'])
    set_text_field("StartTime",row['StartTime'])
    set_text_field("EndTime",row['EndTime'])

    if(row['Category'] == 1):
        set_text_field("CommitRepository",row['Commit'])
    
    set_text_field("Description",row['Description'])

    driver.find_element(By.CLASS_NAME, 'btn-green').click()

    time.sleep(3)

