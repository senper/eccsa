import { useState, useEffect } from "react";
import { Carousel } from "react-bootstrap";
import { getAdImages } from "../../api/advert"

const advertHereList = ["bottomad1.gif", "bottomad2.gif"];
export default function TopAdvert() {
  const [adImages, setAdImages] = useState([]);
  useEffect(() => {
    getAdImages(1).then((data) => {
      setAdImages(data.data);
    });
  }, []);
  return (
    <Carousel variant="light" indicators={false} style={{ paddingTop: "20px" }}>
      {adImages && adImages.$values && adImages.$values.length > 0
        ? adImages.$values.map((img, index) => (
            <Carousel.Item key={index}>
              <img
                src={
                  process.env.NEXT_PUBLIC_API_URL +
                  "/AdvertisingImages/header/" +
                  img
                }
                style={{
                  width: "100%",
                  height: "100%",
                  maxHeight: "78px",
                }}
                alt={img}
            />
            </Carousel.Item>
          ))
        : advertHereList.map((img, index) => (
            <Carousel.Item key={index}>
              <img
                src={"/AdvertisingImages/" + img}
                style={{ width: "100%", height: "100%" }}
                alt={img}
            />
            </Carousel.Item>
          ))}
    </Carousel>
  );
}
