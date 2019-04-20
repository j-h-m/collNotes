dwc = ["recordNumber","siteNumber","specimenNumber","genericcolumn2","associatedCollectors","habitat","individualCount",
       "reproductiveCondition","locality","locationRemarks","occurrenceRemarks","recordedBy","Label Project","samplingEffort",
       "substrate","associatedTaxa","eventDate","establishmentMeans","genericcolumn1","decimalLatitude","decimalLongitude",
       "coordinateUncertaintyInMeters","minimumElevationInMeters","scientificName","scientificNameAuthorship","country","stateProvince","county","path"]

# generate column order code for GetColumnNames function
for idx, el in enumerate(dwc):
    if idx < len(dwc) - 1:
        print ('{ ' + str(idx) + ', "' + el + '" },')
    else:
        print ('{ ' + str(idx) + ', "' + el + '" }')


count = 0
while count <= 28:
    print ('{' + str(count) + ', new KeyValuePair<Type, string>(typeof(Class), "property") }, // ' + dwc[count])
    count += 1
