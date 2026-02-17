import { useState, useEffect } from "react";
import { Carousel } from "react-bootstrap";
import { getAdImages } from "../../api/advert";

const advertHereList = ["ad1.jpg", "ad1.jpg"];
export default function BottomAdvert() {
  const [adImages, setAdImages] = useState([]);
  useEffect(() => {
    getAdImages(2).then((data) => {
      setAdImages(data.data);
    });
  }, []);
  return (
    <Carousel variant="light" indicators={false}>
          {adImages && adImages.$values && adImages.$values.length > 0
            ? adImages.$values.map((img, index) => (
                <Carousel.Item key={index}>
                  <img
                    alt={"Advert"}
                    src={
                      process.env.NEXT_PUBLIC_API_URL +
                      "/AdvertisingImages/footer/" +
                      img
                    }
                    style={{
                      width: "100%",
                      height: "100%",
                      maxHeight: "80px",
                    }}
                  ></img>
                </Carousel.Item>
              ))
            : advertHereList.map((img, index) => (
                <Carousel.Item key={index}>
                  <img
                    alt={"Advert"}
                    src={"/AdvertisingImages/" + img}
                    style={{
                      width: "100%",
                      height: "100%",
                      maxHeight: "80px",
                    }}
                  ></img>
                </Carousel.Item>
              ))}
        </Carousel>
  );
}
